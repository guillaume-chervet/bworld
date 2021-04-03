using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Core.Models.Page;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.Command.News
{
    public class NewsItemBusinessModule : BusinessModuleBase
    {
        private readonly UrlProvider _urlProvider;

        public NewsItemBusinessModule(UrlProvider urlProvider, BusinessModuleFactory businessModuleFactory) : base(businessModuleFactory)
        {
            _urlProvider = urlProvider;
        }

        public const string ModuleName = "NewsItem";

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new[]
            {
                new Route
                {
                    Identity = "1",
                    Action = "Index",
                    Controller = ModuleName,
                    Path = "{private}/articles/item/{moduleid}/{title}",
                    Regex = @"^.+\/articles\/item\/.+\/.+"
                },
                new Route
                {
                    Identity = "2",
                    Action = "Index",
                    Controller = ModuleName,
                    Path = "articles/item/{moduleid}/{title}",
                    Regex = @"^articles\/item\/.+\/.+"
                },
                new Route
                {
                    Identity = "3",
                    Action = "Admin",
                    Controller = ModuleName,
                    Path = "administration/articles/item/{moduleid}/{title}",
                    Regex = @"^administration\/articles\/item\/.+\/.+$"
                }
            };
        }


        public override async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            return
                await
                    SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory, dataFactorySource,
                        dataFactoryDestination, itemDataModelSource,
                        itemDataModelDestinationParent, isTransfert, data);
        }

        #region Génération du menu associé au module

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var currentRequest = geMenuItemInput.CurrentRequest;

            var propertyName = itemDataModel.PropertyName;
            var isPrivate = IsPrivate(propertyName);

            if (isPrivate && geMenuItemInput.IsSitemap)
            {
                return;
            }

            var menuItem = await GetMenuItemAsync(_urlProvider, itemDataModel, currentRequest, ModuleName, isPrivate);
            if (menuItem != null)
            {
                ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, menuItem,
                    itemDataModel.PropertyType);
            }
        }

        public static bool IsPrivate(string propertyName)
        {
            var isPrivate = !string.IsNullOrEmpty(propertyName) && propertyName.ToLower().StartsWith("private");
            return isPrivate;
        }

        public static async Task<MenuItemBusiness> GetMenuItemAsync(UrlProvider urlProvider,
            ItemDataModel itemDataModel, ICurrentRequest currentRequest, string moduleName, bool isPrivate = false)
        {
            var moduleId = itemDataModel.Id;

            var freeBusinessModel = (ElementBusinessModel) itemDataModel.Data;
            var elements = freeBusinessModel.Elements;
            var title = FreeBusinessModule.GetTitle(elements);
            var normalizedTitle = UrlHelper.NormalizeTextForUrl(title);

            var propertyName = itemDataModel.PropertyName;
            object o = new {title = normalizedTitle, moduleId};
            if (isPrivate)
            {
                o = new {title = normalizedTitle, moduleId, @private = "privee"};
            }

            var urlInfo = await urlProvider.GetUrlAsync(currentRequest, "Index", moduleName, o);
            var menuItem = new MenuItemBusiness();
            menuItem.Title = title;
            menuItem.RouteDatas = GetRouteData("Index", moduleName, normalizedTitle, moduleId, propertyName);
            menuItem.Url = urlInfo.Path;
            menuItem.RoutePath = urlInfo.RoutePath;
            menuItem.RoutePathWithoutHomePage = urlInfo.RoutePathWithoutHomePage;
            menuItem.Route = urlInfo.Route;
            menuItem.ModuleId = moduleId;
            menuItem.ModuleName = moduleName;
            menuItem.Icon = freeBusinessModel.Icon;
            menuItem.State = itemDataModel.State;
            menuItem.Seo = new SeoBusiness();
            menuItem.Seo.UpdateDate = itemDataModel.UpdateDate.HasValue
                ? itemDataModel.UpdateDate.Value
                : itemDataModel.CreateDate;
            menuItem.Seo.SitemapFrequence = SitemapFrequence.Weekly;
            menuItem.Seo.SocialImageUrl =
                FreeBusinessModule.GetFirstImageUrl(itemDataModel.SiteId, freeBusinessModel);

            var metaKeywords = elements.FirstOrDefault(e => e.Property == "MetaKeyword");
            if (metaKeywords != null)
            {
                elements.Remove(metaKeywords);
            }
            // TODO calculer en automatique
            menuItem.Seo.MetaKeyword = "";
            menuItem.Seo.MetaDescription = FreeBusinessModule.GetMetaDescription(elements);

            return menuItem;
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            throw new NotImplementedException();
        }

        public static IDictionary<string, string> GetRouteData(string action, string controller, string title,
            string moduleId, string @private)
        {
            var routaDatas = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(@private))
            {
                routaDatas.Add("privee", @private);
            }

            routaDatas.Add("action", action);
            routaDatas.Add("controller", controller);
            routaDatas.Add("title", title);
            routaDatas.Add("moduleId", moduleId);

            return routaDatas;
        }

        #endregion
    }
}