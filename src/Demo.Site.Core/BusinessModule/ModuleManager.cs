using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using Demo.Business.Routing;
using Demo.Common;
using Demo.Data;
using Demo.Data.Model.Cache;
using Demo.Data.Model.Web;
using Demo.Data.Repository;
using Newtonsoft.Json;

namespace Demo.Business.BusinessModule
{
    public class ModuleManager
    {
        private readonly IDataFactory _dataFactory;
        private readonly CacheProvider _cacheProvider;

        public ModuleManager(IDataFactory dataFactory, CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _cacheProvider = cacheProvider;
        }

        public async Task<ExpandoObject> GetMasterAsync(ICurrentRequest currentRequest)
        {
            var cacheItemValue =
                await _dataFactory.CacheRepository.GetValueAsync<ExpandoObject>(currentRequest, CacheRepository.CacheMasterKey);
            if (cacheItemValue == null)
            {
                var master = await _cacheProvider.GetMasterAsync(currentRequest);

                var cacheItem = new CacheItem();
                cacheItem.Value = JsonConvert.SerializeObject(master);
                cacheItem.Key = JsonConvert.SerializeObject(currentRequest);
                cacheItem.Type = CacheRepository.CacheMasterKey;
                cacheItem.SiteId = currentRequest.SiteId;
                cacheItem.CreateDate = DateTime.Now;
                await _dataFactory.CacheRepository.SaveAsync(cacheItem);

                return master;
            }
            // cacheItem.MasterJson
            //Json. ExpandoObject
            return cacheItemValue;
        }

        public async Task<ExpandoObject> GetSitemapAsync(ICurrentRequest currentRequest)
        {
            var cacheItemValue =
                await _dataFactory.CacheRepository.GetValueAsync<ExpandoObject>(currentRequest, "Sitemap");
            if (cacheItemValue == null)
            {
                var sitemap = await _cacheProvider.GetSiteMapAsync(currentRequest);

                var cacheItem = new CacheItem();
                cacheItem.Value = JsonConvert.SerializeObject(sitemap);
                cacheItem.Key = JsonConvert.SerializeObject(currentRequest);
                cacheItem.Type = "Sitemap";
                cacheItem.SiteId = currentRequest.SiteId;
                cacheItem.CreateDate = DateTime.Now;
                await _dataFactory.CacheRepository.SaveAsync(cacheItem);

                return sitemap;
            }
            // cacheItem.MasterJson
            //Json. ExpandoObject
            return cacheItemValue;
        }

        public static void Add(IDictionary<string, object> masterDictionnary, string propertyName, object value,
            PropertyType propertyType)
        {
            if (masterDictionnary == null) return;
            if (value == null) return;

            if (propertyType == PropertyType.Property)
            {
                DicoHelper.AddObject(masterDictionnary, propertyName, value);
            }
            else if (propertyType == PropertyType.List)
            {
                IList list;
                if (masterDictionnary.ContainsKey(propertyName))
                {
                    list = (IList) masterDictionnary[propertyName];
                }
                else
                {
                    list = new ArrayList();
                    masterDictionnary.Add(propertyName, list);
                }
                list.Add(value);
            }
        }
    }
}