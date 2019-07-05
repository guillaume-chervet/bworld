using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Model.Cache;
using Demo.Data.Model.Web;
using Demo.Data.Repository;
using Demo.Routing.Extentions;
using Demo.Routing.Interfaces;
using Newtonsoft.Json;

namespace Demo.Business
{
    public class CacheProvider
    {
        private readonly IDataFactory _dataFactory;
        private readonly BusinessModuleFactory _businessFactory;
        private readonly IRouteProvider _routeProvider;

        public CacheProvider(IDataFactory dataFactory, BusinessModuleFactory businessFactory, IRouteProvider routeProvider)
        {
            _dataFactory = dataFactory;
            _businessFactory = businessFactory;
            _routeProvider = routeProvider;
        }

        public async Task InitializeCacheAsync()
        {
            await _dataFactory.CacheRepository.ClearAsync();
            await CacheItemsAsync();
            await CacheItemsAsync("Sites");
           // await CacheItemsAsync("bworld");
            //await CacheItemsAsync("mylocalworld");
            //await CacheItemsAsync("foodtruckauradisgourmand");
        }

        public async Task RemoveCacheAsync(string siteId)
        {
            await _dataFactory.CacheRepository.DeleteAsync(siteId);
        }

        public async Task UpdateCacheAsync(string siteId)
        {
            await _dataFactory.CacheRepository.DeleteAsync(siteId);
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(null, siteId);
            var siteDataModel = (SiteBusinessModel) itemDataModel.Data;

            var getSiteId = new GetSiteId();    
            getSiteId.MasterDomainId = siteDataModel.MasterDomainId;
            getSiteId.Data = new Dictionary<string, string>();
            getSiteId.Data.Add("site", UrlHelper.NormalizeTextForUrl(siteDataModel.Name));

            IList<CacheItem> cacheItems = new List<CacheItem>(3);

            var key = JsonConvert.SerializeObject(getSiteId);
            var site = new CacheItem
            {
                SiteId = siteId,
                Type = CacheRepository.CacheRouteKey,
                Key = key,
                Value = JsonConvert.SerializeObject(await GetRootMetadataAsync(siteId)),
                CreateDate = DateTime.Now
            };

            cacheItems.Add(site);

            var siteSeo = new CacheItem
            {
                SiteId = siteId,
                Type = CacheRepository.CacheSeoKey,
                Key = "Seo",
                Value = JsonConvert.SerializeObject(false),
                CreateDate = DateTime.Now
            };

            cacheItems.Add(siteSeo);

            await _dataFactory.CacheRepository.SaveAsync(cacheItems);
        }

        public async Task<IEnumerable<CacheItem>> CacheItemsAsync(string masterDomainId = "")
        {
            var sites = new List<CacheItem>();

            var siteRepository = _dataFactory.ItemRepository;
            var _siteDataModels = await siteRepository.GetItemsAsync(null, new ItemFilters {Module = "Site"});

            foreach (var itemDataModel in _siteDataModels)
            {
                var siteDataModel = (SiteBusinessModel) itemDataModel.Data;
                if ( string.IsNullOrEmpty(masterDomainId) || siteDataModel.MasterDomainId == masterDomainId)
                {
                    var siteId = itemDataModel.Id.ToString(CultureInfo.InvariantCulture);

                    var getSiteId = new GetSiteId();
                    getSiteId.MasterDomainId = string.IsNullOrEmpty(masterDomainId) ? siteDataModel.MasterDomainId: masterDomainId;
                    getSiteId.Data = new Dictionary<string, string>();
                    getSiteId.Data.Add("site", UrlHelper.NormalizeTextForUrl(siteDataModel.Name));
                    if (!string.IsNullOrEmpty(siteDataModel.Domain))
                    {
                        getSiteId.Data.Add("domain", UrlHelper.NormalizeTextForUrl(siteDataModel.Domain));
                    }

                    var key = JsonConvert.SerializeObject(getSiteId);
                    var site = new CacheItem
                    {
                        SiteId = siteId,
                        Type = CacheRepository.CacheRouteKey,
                        Key = key,
                        Value = JsonConvert.SerializeObject(await GetRootMetadataAsync(siteId)),
                        CreateDate = DateTime.Now
                    };

                    sites.Add(site);

                    var sitePrivate = new CacheItem
                    {
                        SiteId = siteId,
                        Type = CacheRepository.CacheRouteKey,
                        Key = key,
                        Value = JsonConvert.SerializeObject(await GetRootMetadataAsync(siteId, "PrivateMenuItems")),
                        CreateDate = DateTime.Now
                    };

                    sites.Add(sitePrivate);

                    await _dataFactory.CacheRepository.SaveAsync(site);

                    var siteSeo = new CacheItem
                    {
                        SiteId = siteId,
                        Type = CacheRepository.CacheSeoKey,
                        Key = "Seo",
                        Value = JsonConvert.SerializeObject(false),
                        CreateDate = DateTime.Now
                    };

                    await _dataFactory.CacheRepository.SaveAsync(siteSeo);
                }
            }

            return sites;
        }

        public async Task<IDictionary<string, string>> GetRootMetadataAsync(string siteId, string propertyName= "MenuItems")
        {
            var states = new List<ItemState>() {ItemState.Published};
            var itemRepository = _dataFactory.ItemRepository;
            var itemDataModels =
                await
                    itemRepository.GetItemsAsync(siteId,
                        new ItemFilters {ParentId = siteId, PropertyName = propertyName, Limit = 1, States = states});

            foreach (var itemDataModel in itemDataModels)
            {
                var module = _businessFactory.GetModule(itemDataModel.Module);

                if (module != null)
                {
                    var rootMetadata =  module.GetRootMetadata(
                            new GetRootMetaDataInput
                            {
                                DataFactory = _dataFactory,
                                ItemDataModel = itemDataModel
                            });
                    if (rootMetadata != null)
                    {
                        return rootMetadata;
                    }
                }
            }

            var routaDatas = new Dictionary<string, string>();
            routaDatas.Add("action", "NoPage");
            routaDatas.Add("controller", "Default");

            return routaDatas;
        }

        public async Task<ExpandoObject> GetMasterAsync(ICurrentRequest currentRequest)
        {
            var master = new ExpandoObject();
            var masterDictionnary = (IDictionary<string, object>) master;
            var itemRepository = _dataFactory.ItemRepository;
            var items =
                await
                    itemRepository.GetItemsAsync(currentRequest.SiteId,
                        new ItemFilters {ParentId = currentRequest.SiteId});
            masterDictionnary.Add("Site", currentRequest);
            await GetChildsAsync(_businessFactory, currentRequest, items, masterDictionnary, _dataFactory);

            // On récupère le le domain sur lequel on doit le logger les utilisateurs
            var domain = _routeProvider.Domains.Where(d => d.Id == currentRequest.DomainId && string.IsNullOrEmpty( d.RedirecToDomainId)).FirstOrDefault();
            masterDictionnary.Add("DomainLoginUrl", domain.DomainLoginUrl);

            return master;
        }

        public async Task<ExpandoObject> GetSiteMapAsync(ICurrentRequest currentRequest)
        {
            var master = new ExpandoObject();
            var masterDictionnary = (IDictionary<string, object>) master;
            var itemRepository = _dataFactory.ItemRepository;
            var items =
                await
                    itemRepository.GetItemsAsync(currentRequest.SiteId,
                        new ItemFilters {ParentId = currentRequest.SiteId});
            await GetChildsAsync(_businessFactory, currentRequest, items, masterDictionnary, _dataFactory, true);
            return master;
        }

        public static ExpandoObject ToExpando(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value is ExpandoObject)
            {
                return (ExpandoObject) value;
            }

            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }

        public static async Task GetChildsAsync(BusinessModuleFactory businessModuleFactory, ICurrentRequest currentRequest, IList<ItemDataModel> items,
            IDictionary<string, object> masterDictionnary, IDataFactory dataFactory, bool isSitemap = false)
        {
            if (items != null)
            {
                var allLists =
                    items.Where(i => i.PropertyType == PropertyType.List)
                        .OrderBy(i => i.Index);
                foreach (var item in allLists)
                {
                    var module = businessModuleFactory.GetModule(item.Module);
                    if (module == null)
                    {
                        continue;
                    }

                    await module.GetInfoAsync(new GeMenuItemInput
                    {
                        ItemDataModel = item,
                        CurrentRequest = currentRequest,
                        DataFactory = dataFactory,
                        Master = masterDictionnary,
                        IsSitemap = isSitemap
                    });
                }

                var allProperties =
                    items.Where(i => i.PropertyType == PropertyType.Property)
                        .OrderBy(i => i.Index);
                foreach (var item in allProperties)
                {
                    var module = businessModuleFactory.GetModule(item.Module);
                    if (module == null)
                    {
                        continue;
                    }

                    await module.GetInfoAsync(new GeMenuItemInput
                    {
                        ItemDataModel = item,
                        CurrentRequest = currentRequest,
                        DataFactory = dataFactory,
                        Master = masterDictionnary,
                        IsSitemap = isSitemap
                    });
                }
            }
        }
    }
}