using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Data.Mock;
using Demo.Data.Model;
using Demo.Data.Model.Cache;
using Demo.Data.Repository.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Demo.Data.Repository
{
    public class ItemRepositoryMongo : IItemRepository
    {
        private readonly IMongoCollection<Item> _collection;
        private readonly MemorySession<ItemDataModelBase> _memorySession;
        private readonly IDataBlob _mongoBlob;
        private readonly ICacheRepository _cacheRepository;
        private string finalize = @"
    function(key, value){
      
      return value;

    }";
        private string map = @"
    function() {
        var item = this;

        var size = 0;
        if(item.SizeBytes)
        {
            size = item.SizeBytes;
        }
         if(item._id.toString() == $SiteId$) {
            emit($SiteId$, { count: 1, size: size });
        }   
    }";
        private string reduce = @"        
    function(key, values) {
        var result = {count: 0, totalSize: 0 };

        values.forEach(function(value){               
            result.count += value.count;
            result.totalSize += value.size;
        });

        return result;
    }";

        public ItemRepositoryMongo(IMongoDatabase database, MemorySession<ItemDataModelBase> memorySession,
            IDataBlob mongoBlob, ICacheRepository cacheRepository)
        {
            _memorySession = memorySession;
            _mongoBlob = mongoBlob;
            _cacheRepository = cacheRepository;
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _collection = database.GetCollection<Item>("site.item", collectionSettings);
        }

        public async Task<IList<ItemDataModel>> GetItemsAsync(string siteId, ItemFilters itemFilters)
        {
            var getItem = new GetItems() {ItemFilters = itemFilters, SiteId = siteId};
            var key = JsonConvert.SerializeObject(getItem);

            var items = await _cacheRepository.UseCacheAsync<List<Item>>(key, "ItemRepository.GetItems", siteId, async () =>
                {
                    var builder = Builders<Item>.Filter;

                    var filter = builder.Empty;

                    if (!string.IsNullOrEmpty(siteId))
                    {
                        filter = filter & builder.Eq(p => p.SiteId, siteId);
                    }

                    if (!string.IsNullOrEmpty(itemFilters.ParentId))
                    {
                        filter = filter & builder.Eq(p => p.ParentId, itemFilters.ParentId);
                    }

                    if (itemFilters.ExcludedModules != null)
                    {
                        foreach (var excludedModule in itemFilters.ExcludedModules)
                        {
                            filter = filter & builder.Ne(p => p.Module, excludedModule);
                        }
                    }

                    if (itemFilters.Tags != null && itemFilters.Tags.Count > 0)
                    {
                        filter = filter & builder.AnyIn("Tags", itemFilters.Tags.ToArray());
                    }

                    if (!string.IsNullOrEmpty(itemFilters.Module))
                    {
                        filter = filter & builder.Eq(p => p.Module, itemFilters.Module);
                    }
                    else if (string.IsNullOrEmpty(itemFilters.ParentId))
                    {
                        filter = filter & builder.Eq(p => p.ParentId, itemFilters.ParentId);
                    }

                    if (!string.IsNullOrEmpty(itemFilters.PropertyName))
                    {
                        filter = filter & builder.Eq(p => p.PropertyName, itemFilters.PropertyName);
                    }

                    if (itemFilters.IsTemporary.HasValue)
                    {
                        filter = filter & builder.Eq(p => p.IsTemporary, itemFilters.IsTemporary.Value);
                    }

                    if (itemFilters.States != null && itemFilters.States.Count >0)
                    {
                        var builderState = builder.Exists(p => p.State, false);
                        foreach (var itemFiltersState in itemFilters.States)
                        {
                            builderState = builderState | builder.Eq(p => p.State, itemFiltersState);
                        }
                        filter = filter & builderState;
                    }

                    if (itemFilters.IndexLte.HasValue)
                    {
                        filter = filter & builder.Lte(p => p.Index, itemFilters.IndexLte.Value);
                    }

                    if (itemFilters.IndexGte.HasValue)
                    {
                        filter = filter & builder.Gte(p => p.Index, itemFilters.IndexGte.Value);
                    }

                    if (itemFilters.IndexLt.HasValue)
                    {
                        filter = filter & builder.Lt(p => p.Index, itemFilters.IndexLt.Value);
                    }

                    if (itemFilters.IndexGt.HasValue)
                    {
                        filter = filter & builder.Gt(p => p.Index, itemFilters.IndexGt.Value);
                    }

                    var cursor = _collection.Find(filter);
                    if (itemFilters.Limit.HasValue)
                    {
                        cursor.Limit(itemFilters.Limit.Value);
                    }
                    cursor.Sort(SetOrder(itemFilters.SortAscending));
                    return await cursor.ToListAsync();
                }
            );
            var result = new List<ItemDataModel>();
            foreach (var item in items)
            {
                var itemDataModel = MapItemDataModel(item, itemFilters.HasTracking);
                result.Add(itemDataModel);
            }

            return result;
        }

        public async Task<long> CountItemsAsync(string siteId, CountItemFilters itemFilters)
        {
            var getItem = new CountItems() { CountItemFilters = itemFilters, SiteId = siteId };
            var key = JsonConvert.SerializeObject(getItem);

            var result = await _cacheRepository.UseCacheAsync<long?>(key, "ItemRepository.CountItems", siteId, async () =>
            {
                var builder = Builders<Item>.Filter;
                var filter = builder.Eq(p => p.SiteId, siteId);

                if (!string.IsNullOrEmpty(itemFilters.ParentId))
                {
                    filter = filter & builder.Eq(p => p.ParentId, itemFilters.ParentId);
                }

                if (!string.IsNullOrEmpty(itemFilters.Module))
                {
                    filter = filter & builder.Eq(p => p.Module, itemFilters.Module);
                }
                else if (string.IsNullOrEmpty(itemFilters.ParentId))
                {
                    filter = filter & builder.Eq(p => p.ParentId, itemFilters.ParentId);
                }

                if (itemFilters.Tags != null && itemFilters.Tags.Count > 0)
                {
                    filter = filter & builder.AnyIn("Tags", itemFilters.Tags.ToArray());
                }

                if (!string.IsNullOrEmpty(itemFilters.PropertyName))
                {
                    filter = filter & builder.Eq(p => p.PropertyName, itemFilters.PropertyName);
                }

                if (itemFilters.IsTemporary.HasValue)
                {
                    filter = filter & builder.Eq(p => p.IsTemporary, itemFilters.IsTemporary.Value);
                }

                if (itemFilters.States != null && itemFilters.States.Count > 0)
                {
                    var builderState = builder.Exists(p => p.State, false);
                    foreach (var itemFiltersState in itemFilters.States)
                    {
                        builderState = builderState | builder.Eq(p => p.State, itemFiltersState);
                    }
                    filter = filter & builderState;
                }

                if (itemFilters.IndexLte.HasValue)
                {
                    filter = filter & builder.Lte(p => p.Index, itemFilters.IndexLte.Value);
                }

                if (itemFilters.IndexGte.HasValue)
                {
                    filter = filter & builder.Gte(p => p.Index, itemFilters.IndexGte.Value);
                }

                if (itemFilters.IndexLt.HasValue)
                {
                    filter = filter & builder.Lt(p => p.Index, itemFilters.IndexLt.Value);
                }

                if (itemFilters.IndexGt.HasValue)
                {
                    filter = filter & builder.Gt(p => p.Index, itemFilters.IndexGt.Value);
                }

                var cursor = _collection.Find(filter);

                return await cursor.CountAsync();
            });
            return result.HasValue ? result.Value: 0;
        }

        public async Task<ItemDataModel> GetItemAsync(string siteId, string id, bool loadChilds = false,
            bool hasTracking = false, bool sortAscending = true)
        {

            string module = null;
            if (string.IsNullOrEmpty(siteId))
            {
                module = "Site";
                siteId = id;
            }

            var key =
                JsonConvert.SerializeObject(new GetItem()
                {
                    SiteId = siteId,
                    Id = id,
                    LoadChilds = loadChilds,
                    SortAscending = sortAscending
                });
            var item = await _cacheRepository.UseCacheAsync<Item>(key, "ItemRepository.GetItem", siteId, async () =>
            {
                var builder = Builders<Item>.Filter;
                var filter = builder.Empty;

                if (!string.IsNullOrEmpty(module))
                {
                    filter = builder.Eq(x => x.Module, module) & builder.Eq(x => x.Guid, new Guid(id));
                }
                else
                {
                    filter = builder.Eq(x => x.Guid, new Guid(id)) & builder.Eq(x => x.SiteId, siteId);
                }

                return  await _collection.Find(filter).FirstOrDefaultAsync();
        });
            if (item == null)
            {
                return null;
            }

            var itemDataModel = MapItemDataModel(item, hasTracking);

            if (loadChilds)
            {
                var itemFilter = new ItemFilters
                {
                    HasTracking = hasTracking,
                    ParentId = item.Id,
                    SortAscending = sortAscending
                };
                var items = await GetItemsAsync(item.SiteId, itemFilter);
                foreach (var dataModel in items)
                {
                    dataModel.Parent = itemDataModel;
                }
            }
            return itemDataModel;
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string parentdId, string propertyName, string module,
            bool isGetStream = true, bool isGetUrl = false)
        {
            return _mongoBlob.DownloadAsync(siteId, parentdId, propertyName, module, isGetStream, isGetUrl);
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true,
            bool isGetUrl = false)
        {
            return _mongoBlob.DownloadAsync(siteId, id, isGetStream, isGetUrl);
        }

        public Task<IList<FileDataModel>> DownloadsAsync(string siteId, string parentdId, bool isGetStream = false,
            bool isGetUrl = false)
        {
            return _mongoBlob.DownloadsAsync(siteId, parentdId, isGetStream, isGetUrl);
        }

        public Task<IList<FileDataModel>> GetFilesAsync(string siteId, string parentId)
        {
            return _mongoBlob.DownloadsAsync(siteId, parentId, false);
        }

        public async Task<int> GetMaxChildIndexAsync(string siteId, string parentId)
        {
            var builder = Builders<Item>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Eq(x => x.ParentId, parentId);
            var sort = Builders<Item>.Sort.Descending(x => x.Index);
            var projection = Builders<Item>.Projection.Expression(x => new {Index = x.Index});
            var item = await _collection.Find(filter)
                .Sort(sort).Project(projection).Limit(1).FirstOrDefaultAsync();

            if (item == null)
            {
                return 0;
            }
            return item.Index;
        }

        private SortDefinition<Item> SetOrder(bool sortAscending)
        {
            if (sortAscending)
            {
                return Builders<Item>.Sort.Ascending(x => x.Index);
            }
            return Builders<Item>.Sort.Descending(x => x.Index);
        }


        /* //http://odetocode.com/blogs/scott/archive/2012/03/19/a-simple-mapreduce-with-mongodb-and-c.aspx
        public int CountSiteSizeBytes(string siteId)
        {
            var options = new MapReduceArgs();
            options.MapFunction = new BsonJavaScript(map);
            options.ReduceFunction = new BsonJavaScript(reduce);
            options.FinalizeFunction = new BsonJavaScript(finalize);
            options.OutputMode = MapReduceOutputMode.Inline;

            var results = collection.MapReduce(options);

            foreach (var result in results.GetResults())
            {
                Console.WriteLine(result.ToJson());
            }

            return 0;
        }*/

        //http://dotnet.dzone.com/articles/mongodb-aggregation-framework

        public Task<int> CountSiteSizeBytesAsync(string siteId)
        {
            /*
             var match = new BsonDocument 
                { 
                    { 
                        "$match", 
                        new BsonDocument 
                            { 
                                {"SiteId", siteId} 
                            } 
                    } 
                };

            var group = new BsonDocument 
                { 
                    { "$group", 
                        new BsonDocument 
                            { 
                                { "_id", new BsonDocument 
                                             { 
                                                 { "SiteId","$SiteId" }, 
                                             } 
                                }, 
                                { 
                                    "TotalSizeBytes", new BsonDocument 
                                                 { 
                                                     { "$sum", "$SizeBytes" } 
                                                 } 
                                } 
                            } 
                  } 
                };

            var project = new BsonDocument 
                { 
                    { 
                        "$project", 
                        new BsonDocument 
                            { 
                                {"_id", 0}, 
                                {"SiteId", "$_id.SiteId"}, 
                                {"TotalSizeBytes", 1}, 
                            } 
                    } 
                };

            var pipeline = new[] { match, group, project };
            var result = await _collection.AggregateAsync(pipeline);

            var countResult = result.Current.FirstOrDefault();

            var total = (int) countResult["TotalSizeBytes"];

            return total;*/
            throw new NotImplementedException();
        }

        private ItemDataModel MapItemDataModel(Item item, bool hasTracking)
        {
            if (item == null)
            {
                return null;
            }

            var itemDataModel = new ItemDataModel(item, hasTracking);

            _memorySession.DatabaseLoaded.Add(itemDataModel);
            return itemDataModel;
        }
        
        /* public async Task<IList<ItemDataModel>> GetItemsFromParentAsync(string parentId, string module = null,
             bool hasTracking = false)
         {
             List<Item> items;
 
             if (!string.IsNullOrEmpty(module))
             {
                 var builder = Builders<Item>.Filter;
                 var filter = builder.Eq(x => x.Module, module) & builder.Eq(x => x.ParentId, parentId);
                 var sort = Builders<Item>.Sort.Ascending(x => x.Index);
                 items = await _collection.Find(filter)
                     .Sort(sort).ToListAsync();
             }
             else
             {
                 var builder = Builders<Item>.Filter;
                 var filter = builder.Eq(x => x.ParentId, parentId);
                 var sort = Builders<Item>.Sort.Ascending(x => x.Index);
                 items = await _collection.Find(filter)
                     .Sort(sort).ToListAsync();
             }
 
             var result = new List<ItemDataModel>();
             foreach (var item in items)
             {
                 var itemDataModel = MapItemDataModel(item, hasTracking);
                 result.Add(itemDataModel);
             }
 
             return result;
         }
 
         public async Task<ItemDataModel> GetItemFromParentAsync(string parentId, string moduleName, string propertyName,
             bool hasTracking = false)
         {
             var builder = Builders<Item>.Filter;
             var filter = builder.Eq(x => x.PropertyName, propertyName) & builder.Eq(x => x.ParentId, parentId);
             var item = await _collection.Find(filter).FirstOrDefaultAsync();
 
             var itemDataModel = MapItemDataModel(item, hasTracking);
             return itemDataModel;
         }*/
    }
}