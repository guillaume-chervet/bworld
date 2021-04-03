using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Mvc.Core.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Data
{
    public class DataFactoryMongo : IDataFactory
    {
        private readonly DataConfig _dataConfig;
        private readonly IDataBlob _mongoBlob;
        private readonly IMongoDatabase _database;
        private readonly ICacheRepository _cacheRepository;
        private readonly MemorySession<ItemDataModelBase> _itemMemorySession = new MemorySession<ItemDataModelBase>();

        public DataFactoryMongo(IDatabase db, IDataBlob mongoBlob, IOptions<DataConfig> dataConfig)
        {
            _dataConfig = dataConfig.Value;
            _mongoBlob = mongoBlob;
            _database = db.GetDatabase();

            _cacheRepository =  new CacheRepository(db);
        }

        public void Add<T>(T o) where T : DataModelBase
        {
            _itemMemorySession.DatabaseAdd.Add(o as ItemDataModelBase);
        }

        public void Dispose()
        {
        }

        public async Task DeleteAsync<T>(T o) where T : DataModelBase
        {
            if (_itemMemorySession.DatabaseDelete.Count(d => d.Id == o.Id) <= 0)
            {
                var itemDataModel = o as ItemDataModel;
                if (itemDataModel != null)
                {
                    _itemMemorySession.DatabaseDelete.Add(itemDataModel);

                    var siteId = itemDataModel.SiteId;
                    if (string.IsNullOrEmpty(siteId) && string.IsNullOrEmpty(itemDataModel.ParentId))
                    {
                        // Il s'agit du module site
                        siteId = itemDataModel.Id;
                    }

                    var items =
                        await
                            ItemRepository.GetItemsAsync(siteId, new ItemFilters {ParentId = itemDataModel.Id});
                    foreach (var dataModel in items)
                    {
                        await DeleteAsync(dataModel);
                    }

                    var files = await ItemRepository.GetFilesAsync(siteId, itemDataModel.Id);
                    foreach (var dataModel in files)
                    {
                        _itemMemorySession.DatabaseDelete.Add(dataModel);
                    }
                }
                else
                {
                    var fileDataModel = o as FileDataModel;
                    if (fileDataModel != null)
                    {
                        _itemMemorySession.DatabaseDelete.Add(fileDataModel);
                    }
                }
            }
        }

        public async Task DeleteFileAsync(string id)
        {
            var gridFs = new GridFSBucket(_database);
            await gridFs.DeleteAsync(new ObjectId(id));
        }

        public async Task DeleteAsync<T>(string siteId, string id) where T : DataModelBase
        {
            var obj = await ItemRepository.GetItemAsync(siteId, id);
            await DeleteAsync(obj);
        }

        public async Task DeleteAsync<T>(ICollection<T> list) where T : DataModelBase
        {
            foreach (var element in list)
            {
                await DeleteAsync(element);
            }
        }

        public async Task SaveChangeAsync()
        {
            await SaveChangeItemsAsync();
        }

        [Obsolete("Remove")]
        public IItemRepository ItemRepository
        {
            get { return new ItemRepositoryMongo(_database, _itemMemorySession, _mongoBlob, _cacheRepository); }
        }

        [Obsolete("Remove")]
        public ICacheRepository CacheRepository
        {
            get { return _cacheRepository; }
        }

        private async Task SaveChangeItemsAsync()
        {
            var collection = _database.GetCollection<Item>("site.item");
           
            var deleteItems = _itemMemorySession.DatabaseDelete;
            foreach (var o in deleteItems)
            {
                if (o is ItemDataModel)
                {
                    var builder = Builders<Item>.Filter;
                    var filter = builder.Eq(x => x.Guid, new Guid(o.Id)) ;
                    await collection.DeleteOneAsync(filter);
                }
                else
                {
                    await _mongoBlob.DeleteAsync(o.Id);
                }
            }
            _itemMemorySession.DatabaseDelete.Clear();

            var saveItems = _itemMemorySession.DatabaseAdd;
            await SaveAddedItemsAsync(collection, saveItems);

            _itemMemorySession.DatabaseAdd.Clear();

            var loadedItems = _itemMemorySession.DatabaseLoaded;
            foreach (var itemDataModel in loadedItems)
            {
                if (!itemDataModel.HasTracking)
                {
                    continue;
                }

                if (!itemDataModel.HasChange)
                {
                    continue;
                }

                if (itemDataModel is ItemDataModel)
                {

                    var builder = Builders<Item>.Filter;
                    var filter = builder.Eq(p => p.Guid, new Guid(itemDataModel.Id));

                    var item = (await collection.FindAsync(filter)).FirstOrDefault();

                    if (item == null)
                    {
                        continue;
                    }

                    item.Index = itemDataModel.Index;
                    item.IsTemporary = itemDataModel.IsTemporary;
                    if (itemDataModel.Data != null)
                    {
                        item.Json = JsonConvert.SerializeObject(itemDataModel.Data);
                        //item.Type = itemDataModel.Data.GetType().FullName;
                    }
                    item.Module = itemDataModel.Module;
                    item.ParentId = itemDataModel.ParentId;
                    item.PropertyName = itemDataModel.PropertyName;
                    item.SiteId = itemDataModel.SiteId;
                    item.UpdateDate = DateTime.Now;
                    item.Id = itemDataModel.Id;
                    item.State = itemDataModel.State;
                    item.Tags = ((ItemDataModel)itemDataModel).Tags;

                    if (itemDataModel.IsLoadedFromDatabase)
                    {
                        await collection.ReplaceOneAsync(new BsonDocument("_id", new Guid(itemDataModel.Id)), item);
                    }
                    else
                    {
                        await collection.InsertOneAsync(item);
                    }
                }
                else
                {
                    throw new NotImplementedException("File update not implemented");
                }
            }
        }

        /// <summary>
        ///     Récuprence qui permet de sauvegarder les enfant avant les parent
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="addedItems"></param>
        /// <param name="count"></param>
        private async Task SaveAddedItemsAsync(IMongoCollection<Item> collection, IList<ItemDataModelBase> addedItems, int count = 0)
        {
            // Sécurité au cas ou on tourne en boucle
            if (count > 100)
            {
                throw new Exception("SaveAddedItemsAsync tourne en boucle, il doit y avoir un bug");
            }

            // Ont sauvegarde en remier tous les parents
            IList<ItemDataModelBase> notSaved = new List<ItemDataModelBase>();
            IList<Item> itemsToSave = new List<Item>();
            IList<Item> itemsToInsert = new List<Item>();
            foreach (var itemDataModel in addedItems)
            {
                if (itemDataModel.Parent != null && string.IsNullOrEmpty(itemDataModel.ParentId))
                {
                    notSaved.Add(itemDataModel);
                    continue;
                }
              
                if (itemDataModel is ItemDataModel)
                {
                    var casted = (ItemDataModel) itemDataModel;
                    var item = MapItemDataModelToItem(casted);
                    if (casted.IsLoadedFromDatabase)
                    {
                        itemsToSave.Add(item);
                    }
                    else
                    {
                        itemsToInsert.Add(item);
                    }
                }
                else
                {
                    await _mongoBlob.UploadAsync((FileDataModel) itemDataModel);
                }
            }

            if (itemsToInsert.Count > 0)
            {
                await collection.InsertManyAsync(itemsToInsert);
            }
            if (itemsToSave.Count > 0)
            {
                foreach (var itemDataModelBase in itemsToSave)
                {
                    await collection.ReplaceOneAsync(new BsonDocument("_id", new Guid(itemDataModelBase.Id)), itemDataModelBase);
                }
            }

            if (notSaved.Count > 0)
            {
                count += 1;
                await SaveAddedItemsAsync(collection, notSaved, count);
            }
        }

        public static Item MapItemDataModelToItem(ItemDataModel itemDataModel)
        {
            if (itemDataModel == null)
            {
                return null;
            }

            var item = new Item();
            item.Index = itemDataModel.Index;
            item.IsTemporary = itemDataModel.IsTemporary;
            if (itemDataModel.Data != null)
            {
                item.Json = JsonConvert.SerializeObject(itemDataModel.Data);
                item.SizeBytes = Encoding.UTF8.GetByteCount(item.Json);
                //item.Type = itemDataModel.Data.GetType().FullName;
            }
            item.Module = itemDataModel.Module;
            item.ParentId = itemDataModel.ParentId;
            item.PropertyName = itemDataModel.PropertyName;
            item.SiteId = itemDataModel.SiteId;
            item.CreateDate = DateTime.Now;
            if (string.IsNullOrEmpty(itemDataModel.Id))
            {
                var id = (Guid.NewGuid()).ToString();
                item.Id = id;
                itemDataModel.Id = id;
                if (String.IsNullOrEmpty(item.SiteId))
                {
                    item.SiteId = id;
                    itemDataModel.SiteId = id;
                }

            }
            else
            {
                item.Id = itemDataModel.Id;
            }
            return item;
        }
        
    }
}