using Demo.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Data.Model.Cache;

namespace Demo.Business.Tests
{
    public class CacheRepositoryMock : ICacheRepository
    {
        public Task ClearAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string siteId)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetFromSiteIdAsync<T>(string siteId, string type)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSiteIdFromKeyAsync(object key, string type)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetValueAsync<T>(object key, string type)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetValueAsync<T>(object key, string type, bool inMemoryOnly = false)
        {
            throw new NotImplementedException();
        }

        public Task<IList<CacheItem>> ListAsync(string type)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string cacheItemId)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(CacheItem cacheItem)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(CacheItem cacheItem, bool inMemoryOnly = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> UseCacheAsync<T>(string key, string type, string region, Func<Task<T>> func)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(IList<CacheItem> cacheItems, bool inMemoryOnly = false)
        {
            throw new NotImplementedException();
        }
    }
}
