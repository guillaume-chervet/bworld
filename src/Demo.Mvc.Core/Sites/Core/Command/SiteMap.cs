using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Models;
using Demo.Mvc.Core.Sites.Core.Command.Site.Master;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command
{
    public class SiteMap
    {
        
        public static async Task<GetSiteMapResult> SitemapUrlAsync(ItemDataModel itemDataModel, IDataFactory dataFactory, IRouteManager routeManager)
        {
            var isSecure = true;
            var siteBusinessModel = (SiteBusinessModel) itemDataModel.Data;

            var input = new FindPathInput();
            input.DomainDatas = new Dictionary<string, string>();
            input.DomainDatas.Add("site", UrlHelper.NormalizeTextForUrl(siteBusinessModel.Name));
            input.MasterDomainId = siteBusinessModel.MasterDomainId;
            input.IsSecure = null;

            input.Datas = new Dictionary<string, string>();
            input.Datas.Add("controller", "Seo");
            input.Datas.Add("action", "Sitemap");

            var result = await routeManager.FindDomainPathAsync(input);

            var getSiteMapResult = new GetSiteMapResult();
            getSiteMapResult.BaseUrl = UrlHelper.Concat(result.BaseUrl, result.PreUrl);
            getSiteMapResult.Url = result.FullUrl;

            return getSiteMapResult;
        }
        
        public static async Task<Site> SiteUrlAsync(IRouteManager routeManager, IDataFactory dataFactory, string siteId)
        {
            var siteRepository = dataFactory.ItemRepository;
            var itemDataModel = await siteRepository.GetItemAsync(null, siteId);

            var sitemap = await SitemapUrlAsync(itemDataModel, dataFactory, routeManager);
            var siteBusinessModel = (SiteBusinessModel) itemDataModel.Data;
            var siteUrl = sitemap.BaseUrl;
            var siteName = siteBusinessModel.Name;

            var itemMasterDataModel = (await
                    dataFactory.ItemRepository.GetItemsAsync(siteId, new ItemFilters { ParentId = siteId, Module = MasterBusinessModule.ModuleName }))
                .FirstOrDefault();
            if (itemMasterDataModel != null)
            {
                var masterBusinessModel =  (MasterBusinessModel) itemMasterDataModel.Data;
                siteName= masterBusinessModel.Elements.First(e => e.Property == "Title").Data;
            }

            return new Site {Name = siteName, Url = siteUrl};
        }
        
        public class Site
        {
            public string Name { get; set; }
            public string Url { get; set; }
        }
        
        public static async Task<string> GetSiteNameAsync(IDataFactory dataFactory, string siteId)
        {
            var siteRepository = dataFactory.ItemRepository;
            var siteDataModel = await siteRepository.GetItemAsync(null, siteId);
            var siteBusinessModel = (SiteBusinessModel) siteDataModel.Data;
            return siteBusinessModel.Name;
        }
    }
}