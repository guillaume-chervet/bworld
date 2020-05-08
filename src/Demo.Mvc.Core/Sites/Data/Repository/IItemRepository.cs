using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Data.Repository
{
    public interface IItemRepository
    {
        Task<int> GetMaxChildIndexAsync(string siteId, string parentId);
        Task<IList<ItemDataModel>> GetItemsAsync(string siteId, ItemFilters itemFilters);
        Task<long> CountItemsAsync(string siteId, CountItemFilters itemFilters);

        Task<ItemDataModel> GetItemAsync(string siteId, string id = null, bool loadChild = false,
            bool hasTracking = false, bool sortAscending = true);

        Task<int> CountSiteSizeBytesAsync(string siteId);
        Task<FileDataModel> DownloadAsync(string siteId, string parentdId, string propertyName, string module, bool isGetStream = true, bool isGetUrl = false);
        Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true, bool isGetUrl = false);
        Task<IList<FileDataModel>> DownloadsAsync(string siteId, string parentdId, bool isGetStream = false, bool isGetUrl = false);
        Task<IList<FileDataModel>> GetFilesAsync(string siteId, string parentId);
    }
}