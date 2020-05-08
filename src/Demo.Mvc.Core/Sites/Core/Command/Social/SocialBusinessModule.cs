using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.News;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Core.Models.Page;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.Command.Social
{
    public class SocialBusinessModule : BusinessModuleBase
    {
        private readonly UrlProvider _urlProvider;
        public const string Url = "social/{moduleid}/{title}";
        public const string UrlPrivate = "{private}/social/{moduleid}/{title}";
        public const string ModuleName = "Social";

        public SocialBusinessModule(BusinessModuleFactory businessModuleFactory, UrlProvider urlProvider) : base(businessModuleFactory)
        {
            _urlProvider = urlProvider;
        }

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            if (geMenuItemInput.IsSitemap)
            {
                return;
            }

            var itemDataModel = geMenuItemInput.ItemDataModel;
            var moduleId = itemDataModel.Id;
            var currentRequest = geMenuItemInput.CurrentRequest;
            var moduleSocial = (SocialBusinessModel) geMenuItemInput.ItemDataModel.Data;

            string title;

            if (string.IsNullOrEmpty(moduleSocial.Title))
            {
                switch (moduleSocial.Socials)
                {
                    case Socials.Phone:
                        title = moduleSocial.Url;
                        break;
                    case Socials.Email:
                        title = moduleSocial.Url;
                        break;
                    default:
                        title = moduleSocial.Socials.ToString();
                        break;
                }
            }
            else
            {
                title = moduleSocial.Title;
            }
                 
            var normalizedTitle = UrlHelper.NormalizeTextForUrl(title);

            var isPrivate = NewsItemBusinessModule.IsPrivate(itemDataModel.PropertyName);
            object o = new { title = normalizedTitle, moduleId };
            if (isPrivate)
            {
                o = new { title = normalizedTitle, moduleId, @private = "privee" };
            }

            var urlInfo = await _urlProvider.GetUrlAsync(currentRequest, "Index", ModuleName, o);

            var menuItem = new MenuItemBusiness();
            menuItem.Title = title;
            menuItem.RouteDatas = NewsItemBusinessModule.GetRouteData(moduleSocial.Socials.ToString(), ModuleName, normalizedTitle, moduleId, itemDataModel.PropertyName);
            menuItem.TypeMenuItem= TypeMenuItem.Link;
            string socialUrl = String.IsNullOrEmpty(moduleSocial.Url) ? "#" : moduleSocial.Url;
            string url = null;
            if (moduleSocial.Socials == Socials.Phone)
            {
                url = "tel:" + socialUrl;
            } else if (moduleSocial.Socials == Socials.Phone)
            {
                url = "mailto:" + socialUrl;
            }
            else
            {
                url = socialUrl;
            }
            menuItem.Url = url;
            menuItem.RoutePath = url;
            menuItem.RoutePathWithoutHomePage = urlInfo.RoutePathWithoutHomePage;
            menuItem.ModuleId = itemDataModel.Id;
            menuItem.ModuleName = ModuleName;

            ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, menuItem, itemDataModel.PropertyType);
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            return null;
        }


        public IDictionary<string, string> GetRootMetadataTemp(GetRootMetaDataInput getRootMetaDataInput)
        {
             var itemDataModel = getRootMetaDataInput.ItemDataModel;
             var moduleId = itemDataModel.Id;
             var moduleFree = (SocialBusinessModel)itemDataModel.Data;

             var title = UrlHelper.NormalizeTextForUrl(moduleFree.Title);

             var routaDatas = new Dictionary<string, string>();
             routaDatas.Add("action", "Index");
             routaDatas.Add("controller", "Social");
             routaDatas.Add("title", title);
             routaDatas.Add("moduleId", moduleId.ToString(CultureInfo.InvariantCulture));

             return routaDatas;
        }

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new[]
         {
                 new Route
                {
                    Identity = "30",
                    Action = "Index",
                    Controller = "Social",
                    Path = UrlPrivate,
                    Regex = @"^.+\/social\/.+\/.+$"
                },
                new Route
                {
                    Identity = "31",
                    Action = "Index",
                    Controller = "Social",
                    Path = Url,
                    Regex = @"^social\/.+\/.+$"
                },
                new Route
                {
                    Identity = "32",
                    Action = "Index",
                    Controller = "Social",
                    Path = "administration/social/{moduleid}/{title}",
                    Regex = @"^administration\/social\/.+\/.+$"
                },
            
            };
        }

        public override async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            return
                await
                    SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory,dataFactorySource, dataFactoryDestination, itemDataModelSource,
                        itemDataModelDestinationParent, isTransfert, data);
        }
    }
}