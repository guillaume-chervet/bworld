using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Sites.Data.Model.Cache;

namespace Demo.Mvc.Core.Sites.Data.Repository
{
    public interface ICacheRepository
    {
        Task ClearAsync();
        Task DeleteAsync(string siteId);
        Task<T> GetFromSiteIdAsync<T>(string siteId, string type);
        Task<string> GetSiteIdFromKeyAsync(object key, string type);
        Task<T> GetValueAsync<T>(object key, string type, bool inMemoryOnly=false);
        Task<IList<CacheItem>> ListAsync(string type);
        Task RemoveAsync(string cacheItemId);
        Task SaveAsync(CacheItem cacheItem, bool inMemoryOnly = false);
        Task SaveAsync(IList<CacheItem> cacheItems, bool inMemoryOnly = false);

        Task<T> UseCacheAsync<T>(string key, string type, string region, Func<Task<T>> func);
    }
}