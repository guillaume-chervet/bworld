using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Business.Command.Free;
using Demo.Business.Command.Free.Models;
using Demo.Business.Models.Page;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Model;
using Demo.Routing.Extentions;
using Demo.Routing.Implementation;

namespace Demo.Business.Command.Site
{
    public class AddSiteBusinessModule : BusinessModuleBase
    {
        private readonly UrlProvider _urlHelper;

        public AddSiteBusinessModule(UrlProvider urlHelper, BusinessModuleFactory businessModuleFactory) : base(businessModuleFactory)
        {
            _urlHelper = urlHelper;
        }

        public const string ModuleName = "AddSite";

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new[]
            {
                new Route
                {
                    Identity = "1",
                    Action = "Index",
                    Controller = "AddSite",
                    Path = "site/{moduleid}/{title}",
                    Regex = @"^site\/.+\/.+$"
                },
                 new Route
                {
                    Identity = "2",
                    Action = "Authentification",
                    Controller = "AddSite",
                    Path = "site/{moduleid}/{title}/authentification?dm=false",
                    Regex = @"^site\/.+\/.+$"
                }
            };
        }


        public override async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            var itemDataModel = await SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory,dataFactorySource, dataFactoryDestination,
                itemDataModelSource, itemDataModelDestinationParent, isTransfert, data);

            return itemDataModel;
        }

        #region Génération du menu associé au module

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var moduleId = itemDataModel.Id;
            var currentRequest = geMenuItemInput.CurrentRequest;

            var menuItem = new MenuItemBusiness();

            var title = GetTitle(itemDataModel);

            var normalizedTitle = UrlHelper.NormalizeTextForUrl(title);

            menuItem.Title = title;
            menuItem.RouteDatas = GetRouteData("Index", ModuleName, normalizedTitle, moduleId);

            var urlInfo = await _urlHelper. GetUrlAsync(currentRequest, "Index", ModuleName,
                new {title = normalizedTitle, moduleId});
            menuItem.Url = urlInfo.Path;
            menuItem.RoutePath = urlInfo.RoutePath;
            menuItem.RoutePathWithoutHomePage = urlInfo.RoutePathWithoutHomePage;
            menuItem.Route = urlInfo.Route;
            menuItem.ModuleId = moduleId;
            menuItem.ModuleName = ModuleName;
            menuItem.Seo = new SeoBusiness();
            menuItem.Seo.UpdateDate = itemDataModel.UpdateDate.HasValue
                ? itemDataModel.UpdateDate.Value
                : itemDataModel.CreateDate;
            menuItem.Seo.SitemapFrequence = SitemapFrequence.Yearly;

            menuItem.Childs = new List<MenuItemBusiness>();

            var menuItemChild = new MenuItemBusiness();
           menuItemChild.Title = title;
            menuItemChild.RouteDatas = GetRouteData("Authentification", ModuleName, normalizedTitle, moduleId);

            var urlInfoChild = await _urlHelper.GetUrlAsync(currentRequest, "Authentification", ModuleName,
                new { title = normalizedTitle, moduleId });
            menuItemChild.Url = urlInfoChild.Path;
            menuItemChild.RoutePath = urlInfoChild.RoutePath;
            menuItemChild.RoutePathWithoutHomePage = urlInfoChild.RoutePathWithoutHomePage;
            menuItemChild.Route = urlInfoChild.Route;
            menuItemChild.ModuleId = moduleId;
            menuItemChild.ModuleName = ModuleName;
            menuItemChild.Seo = new SeoBusiness();
            menuItemChild.Seo.UpdateDate = itemDataModel.UpdateDate.HasValue
                ? itemDataModel.UpdateDate.Value
                : itemDataModel.CreateDate;
            menuItemChild.Seo.SitemapFrequence = SitemapFrequence.Yearly;

            menuItem.Childs.Add(menuItemChild);

            ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, menuItem, itemDataModel.PropertyType);
        }

        private static string GetTitle(ItemDataModel itemDataModel)
        {
            var addSiteBusinessModel = (AddSiteBusinessModel)itemDataModel.Data;
            var title = string.Empty;

            if (addSiteBusinessModel.Elements != null)
            {
                title = FreeBusinessModule.GetTitle(addSiteBusinessModel.Elements);
            }
            if (string.IsNullOrEmpty(title))
            {
                title = "Créer votre site";
            }
            return title;
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            var itemDataModel = getRootMetaDataInput.ItemDataModel;

            var moduleId = itemDataModel.Id;

            var title = UrlHelper.NormalizeTextForUrl(GetTitle(itemDataModel));

            var routaDatas = new Dictionary<string, string>();
            routaDatas.Add("action", "Index");
            routaDatas.Add("controller", ModuleName);
            routaDatas.Add("title", title);
            routaDatas.Add("moduleId", moduleId.ToString(CultureInfo.InvariantCulture));

            return routaDatas;
        }

        private IDictionary<string, string> GetRouteData(string action, string controller, string title, string moduleId)
        {
            var routaDatas = new Dictionary<string, string>();
            routaDatas.Add("action", action);
            routaDatas.Add("controller", controller);
            routaDatas.Add("title", title);
            routaDatas.Add("moduleId", moduleId);

            return routaDatas;
        }

        #endregion
    }
}