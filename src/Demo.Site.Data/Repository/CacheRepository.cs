using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;
using CacheItem = Demo.Data.Model.Cache.CacheItem;
using Demo.Data.Mongo;
using MongoDB.Bson;

namespace Demo.Data.Repository
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IMongoCollection<CacheItem> _collection;
        //private static readonly IList<CacheItem> Items = new System.Collections.Concurrent.  //SynchronizedCollection<CacheItem>();
        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, CacheItem> Items =
            new System.Collections.Concurrent.ConcurrentDictionary<string, CacheItem>();

        public const string CacheMasterKey = "Master";
        public const string CacheRouteKey = "Route";
        public const string CacheUrlRoutageKey = "UrlRoutage";
        public const string CacheSeoKey = "Seo";

        public CacheRepository(IDatabase db)
        {
            var database = db.GetDatabase();
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _collection = database.GetCollection<CacheItem>("site.cache", collectionSettings);
        }


        public async Task SaveAsync(CacheItem cacheItem, bool inMemoryOnly = false)
        {
            cacheItem.Guid = Guid.NewGuid();
            if (!inMemoryOnly)
            {
                await _collection.InsertOneAsync(cacheItem);
            }
            AddMemoryCache(cacheItem);
        }

        public async Task SaveAsync(IList<CacheItem> cacheItems, bool inMemoryOnly = false)
        {
            foreach (var cacheItem in cacheItems)
            {
                cacheItem.Guid = Guid.NewGuid();
                AddMemoryCache(cacheItem);
            }
            
            if (!inMemoryOnly)
            {
                await _collection.InsertManyAsync(cacheItems);
            }
        }

        public async Task DeleteAsync(string siteId)
        {
            var builder = Builders<CacheItem>.Filter;
            var filter = builder.Eq(p => p.SiteId, siteId);
            await _collection.DeleteManyAsync(filter);

            var itemsToRemove = Items.Where(s => s.Value.SiteId == siteId).ToList();
            foreach (var cacheItem in itemsToRemove)
            {
                // TODO
                CacheItem ci;
                Items.TryRemove(CacheItem.GetKey(cacheItem.Value), out ci);
            }
        }

        public async Task RemoveAsync(string cacheItemId)
        {
            var builder = Builders<CacheItem>.Filter;
            var filter = builder.Eq(p => p.Guid, new Guid(cacheItemId));
            await _collection.DeleteOneAsync(filter);

            var itemsToRemove = Items.Where(s => s.Value.Id == cacheItemId).ToList();
            foreach (var cacheItem in itemsToRemove)
            {
                // TODO
                CacheItem ci;
                Items.TryRemove(CacheItem.GetKey(cacheItem.Value) , out ci);
            }
        }
        public async Task ClearAsync()
        {
            var builder = Builders<CacheItem>.Filter;
            var filter = builder.Ne(c => c.SiteId, "");
            await _collection.DeleteManyAsync(filter);

            Items.Clear();
        }

        private static void AddMemoryCache(CacheItem cacheItem)
        {
            if (cacheItem != null)
            {
                Items.TryAdd(cacheItem.Type + ":" + cacheItem.Key, cacheItem);
            }
        }

        public async Task<T> GetFromSiteIdAsync<T>(string siteId, string type)
        {
            var keyValue = Items.FirstOrDefault(s => s.Value.SiteId == siteId && s.Value.Type == type);

            var cacheItem = keyValue.Value;
            if (cacheItem == null)
            {
                var builder = Builders<CacheItem>.Filter;
                var filter = builder.Eq(p => p.SiteId, siteId) & builder.Eq(p => p.Type, type);
                cacheItem = await _collection.Find(filter).FirstOrDefaultAsync();
                AddMemoryCache(cacheItem);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(cacheItem.Value);
            }

            return default(T);
        }

        public async Task<string> GetSiteIdFromKeyAsync(object key, string type)
        {
            var keyJson = JsonConvert.SerializeObject(key);

            CacheItem cacheItem;
            Items.TryGetValue(type + ":" + keyJson, out cacheItem);

            if (cacheItem == null)
            {
                var builder = Builders<CacheItem>.Filter;
                var filter = builder.Eq(p => p.Key, keyJson) & builder.Eq(p => p.Type, type);
                cacheItem = await _collection.Find(filter).FirstOrDefaultAsync();
                AddMemoryCache(cacheItem);
            }

            if (cacheItem != null)
            {
                return cacheItem.SiteId;
            }


            return string.Empty;
        }

        public async Task<T> GetValueAsync<T>(object key, string type, bool inMemoryOnly = false)
        {
            var keyJson = key is string ? (string) key : JsonConvert.SerializeObject(key);

            CacheItem cacheItem;
            Items.TryGetValue(type + ":" + key, out cacheItem);

            if (cacheItem == null && !inMemoryOnly)
            {
                var builder = Builders<CacheItem>.Filter;
                var filter = builder.Eq(p => p.Key, keyJson) & builder.Eq(p => p.Type, type);
                cacheItem = await _collection.Find(filter).FirstOrDefaultAsync();
                AddMemoryCache(cacheItem);
            }

            if (cacheItem != null)
            {
                return JsonConvert.DeserializeObject<T>(cacheItem.Value);
            }

            return default(T);
        }

        public async Task<IList<CacheItem>> ListAsync(string type)
        {
            var builder = Builders<CacheItem>.Filter;
            var filter = builder.Eq(p => p.Type, type);
            var cacheItems = await _collection.Find(filter).ToListAsync();
            return cacheItems;
        }

        public async Task<T> UseCacheAsync<T>(string key, string type, string region, Func<Task<T>> func)
        {
            var items = await this.GetValueAsync<T>(key, type, true);
            if (items == null)
            {
                items = await func();
                await this.SaveAsync(
                    new CacheItem()
                    {
                        CreateDate = DateTime.Now,
                        Type = type,
                        Key = key,
                        Value = JsonConvert.SerializeObject(items),
                        SiteId = region
                    }, true);
                return items;
            }
            return items;
        }
    }
}