using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Data.Mock;
using Demo.Data.Model;

namespace Demo.Data.Repository
{
    public class ItemRepositoryMock : IItemRepository
    {
        private readonly MemorySession<ItemDataModelBase> memorySession;

        public ItemRepositoryMock(MemorySession<ItemDataModelBase> memorySession)
        {
            this.memorySession = memorySession;
        }

        public Task<int> GetMaxChildIndexAsync(string siteId, string parentId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ItemDataModel>> GetItemsAsync(string siteId, ItemFilters itemFilters)
        {
            throw new NotImplementedException();
        }

        public Task<long> CountItemsAsync(string siteId, CountItemFilters itemFilters)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDataModel> GetItemAsync(string siteId, string id = null, bool loadChild = false,
            bool hasTracking = false,
            bool sortAscending = true)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountSiteSizeBytesAsync(string siteId)
        {
            throw new NotImplementedException();
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string parentdId, string propertyName, string module)
        {
            throw new NotImplementedException();
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<FileDataModel>> DownloadsAsync(string siteId, string parentdId, bool isGetStream = false, bool isGetUrl = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<FileDataModel>> GetFilesAsync(string siteId, string parentdId)
        {
            throw new NotImplementedException();
        }

        public IList<ItemDataModel> GetMenuItems(string siteId)
        {
            var items =
                MemoryDatabase.Database
                    .Where(
                        d =>
                            d.SiteId == siteId && d.ParentId == siteId &&
                            d.PropertyName == "MenuItems")
                    .OrderBy(d => d.Index)
                    .ToList();


            var result = new List<ItemDataModel>();
            foreach (var item in items)
            {
                var itemDataModel = MapItemDataModel(item, false);

                result.Add(itemDataModel);
            }

            return result;
        }

        public IList<ItemDataModel> GetItems(string siteId, ItemFilters itemFilters)
        {
            var parentId = itemFilters.ParentId;
            var module = itemFilters.Module;
            var hasTracking = itemFilters.HasTracking;
            var sortAscending = itemFilters.SortAscending;
            List<Item> items;
            if (!string.IsNullOrEmpty(module))
            {
                items =
                    MemoryDatabase.Database
                        .Where(
                            d =>
                                d.SiteId == siteId && d.ParentId == parentId && d.Module == module)
                        .OrderBy(d => d.Index)
                        .ToList();
            }
            else if (!string.IsNullOrEmpty(parentId))
            {
                items =
                    MemoryDatabase.Database
                        .Where(d => d.SiteId == siteId && d.ParentId == parentId)
                        .OrderBy(d => d.Index)
                        .ToList();
            }
            else
            {
                items =
                    MemoryDatabase.Database
                        .Where(d => d.SiteId == siteId)
                        .OrderBy(d => d.Index)
                        .ToList();
            }


            var result = new List<ItemDataModel>();
            foreach (var item in items)
            {
                var itemDataModel = MapItemDataModel(item, hasTracking);

                result.Add(itemDataModel);
            }

            return result;
        }

        public Task<IList<ItemDataModel>> GetItemsAsync(string siteId, string parentId = null, string module = null,
            bool hasTracking = false,
            bool sortAscending = true)
        {
            throw new NotImplementedException();
        }

        public IList<ItemDataModel> GetItemsFromParent(string parentId, string module = null, bool hasTracking = false)
        {
            List<Item> items;
            if (!string.IsNullOrEmpty(module))
            {
                items =
                    MemoryDatabase.Database
                        .Where(
                            d =>
                                d.ParentId == parentId && d.Module == module)
                        .OrderBy(d => d.Index)
                        .ToList();
            }
            else
            {
                items =
                    MemoryDatabase.Database
                        .Where(d => d.ParentId == parentId)
                        .OrderBy(d => d.Index)
                        .ToList();
            }

            var result = new List<ItemDataModel>();
            foreach (var item in items)
            {
                var itemDataModel = MapItemDataModel(item, hasTracking);

                result.Add(itemDataModel);
            }

            return result;
        }

        public ItemDataModel GetItem(string id, bool loadChilds = false, bool hasTracking = false,
            bool sortAscending = true)
        {
            var item = MemoryDatabase.Database.Where(d => d.Id == id).FirstOrDefault();

            if (item == null)
            {
                return null;
            }

            var itemDataModel = MapItemDataModel(item, hasTracking);

            if (loadChilds)
            {
                var items = GetItems(item.SiteId, new ItemFilters {ParentId = item.Id});
                foreach (var dataModel in items)
                {
                    dataModel.Parent = itemDataModel;
                }
            }
            return itemDataModel;
        }

        public ItemDataModel GetItemFromParent(string parentId, string moduleName, string propertyName,
            bool hasTracking = false)
        {
            var item =
                MemoryDatabase.Database.Where(
                    d => d.ParentId == parentId && d.Module == moduleName && d.PropertyName == propertyName)
                    .FirstOrDefault();


            var itemDataModel = MapItemDataModel(item, hasTracking);

            return itemDataModel;
        }

        public int CountSiteSizeBytes(string siteId)
        {
            throw new NotImplementedException("CountSiteSizeBytes");
        }

        private ItemDataModel MapItemDataModel(Item item, bool hasTracking)
        {
            if (item == null)
            {
                return null;
            }

            var itemDataModel = new ItemDataModel();
            itemDataModel.HasTracking = hasTracking;
            itemDataModel.Index = item.Index;
            itemDataModel.IsTemporary = item.IsTemporary;
            itemDataModel.Data = MemoryDatabase.GetItemData(item);
            itemDataModel.Module = item.Module;
            itemDataModel.ParentId = item.ParentId;
            itemDataModel.PropertyName = item.PropertyName;
            itemDataModel.SiteId = item.SiteId;
            itemDataModel.CreateDate = item.CreateDate;
            itemDataModel.UpdateDate = item.UpdateDate;
            itemDataModel.Id = item.Id;

            memorySession.DatabaseLoaded.Add(itemDataModel);
            return itemDataModel;
        }

        public Task<IList<ItemDataModel>> GetMenuItemsAsync(string siteId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ItemDataModel>> GetItemsAsync(string siteId, string parentId = null, string module = null,
            bool hasTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ItemDataModel>> GetItemsFromParentAsync(string parentId, string module = null,
            bool hasTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDataModel> GetItemAsync(string id, bool loadChild = false, bool hasTracking = false,
            bool sortAscending = true)
        {
            throw new NotImplementedException();
        }

        public Task<ItemDataModel> GetItemFromParentAsync(string parentId, string moduleName, string propertyName,
            bool hasTracking = false)
        {
            throw new NotImplementedException();
        }

        public string Upload(Stream stream, string fileName, string siteId = null)
        {
            throw new NotImplementedException();
        }

        public Stream Download(string fileId)
        {
            throw new NotImplementedException();
        }

        public int GetMaxChildIndex(string siteId, string parentId)
        {
            throw new NotImplementedException();
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string parentdId, string propertyName, string module, bool isGetStream = true,
            bool isGetUrl = false)
        {
            throw new NotImplementedException();
        }

        public Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true, bool isGetUrl = false)
        {
            throw new NotImplementedException();
        }

   
    }
}