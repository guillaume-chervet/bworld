using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command.News
{
    public class NewsBusinessModule : BusinessModuleBase
    {
        private readonly UrlProvider _urlProvider;

        public NewsBusinessModule(BusinessModuleFactory businessModuleFactory, UrlProvider urlProvider) : base(businessModuleFactory)
        {
            _urlProvider = urlProvider;
        }

        public const string ModuleName = "News";
        public const string Url = "articles/{moduleid}/{title}";
        public const string UrlPrivate = "{private}/articles/{moduleid}/{title}";

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new[]
            {
                new Route
                {
                    Identity = "1",
                    Action = "Index",
                    Controller = ModuleName,
                    Path = Url,
                    Regex = @"^articles\/.+\/.+$"
                },
                new Route
                {
                    Identity = "2",
                    Action = "Index",
                    Controller = ModuleName,
                    Path = UrlPrivate,
                    Regex = @"^.+\/articles\/.+\/.+$"
                },
                new Route
                {
                    Identity = "3",
                    Action = "Admin",
                    Controller = ModuleName,
                    Path = "administration/articles/{moduleid}/{title}",
                    Regex = @"^administration\/articles\/.+\/.+$"
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
                    SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory,dataFactorySource, dataFactoryDestination, itemDataModelSource,
                        itemDataModelDestinationParent, isTransfert, data);
        }

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var moduleId = itemDataModel.Id;
            var currentRequest = geMenuItemInput.CurrentRequest;
            var propertyName = itemDataModel.PropertyName;

            var isPrivate = NewsItemBusinessModule.IsPrivate(propertyName);

            if (isPrivate && geMenuItemInput.IsSitemap)
            {
                return;
            }
            
            var menuItem = await NewsItemBusinessModule.GetMenuItemAsync(_urlProvider, itemDataModel,  currentRequest, ModuleName, isPrivate);
            if (menuItem != null)
            {
                var expendoMenu = CacheProvider.ToExpando(menuItem);
                {
                    var items =
                        await
                            geMenuItemInput.DataFactory.ItemRepository.GetItemsAsync(itemDataModel.SiteId,
                                new ItemFilters {ParentId = moduleId});
                    await
                        CacheProvider.GetChildsAsync(_businessModuleFactory, geMenuItemInput.CurrentRequest, items,
                            expendoMenu,
                            geMenuItemInput.DataFactory);
                }

                ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, expendoMenu,
                    itemDataModel.PropertyType);
            }
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            var itemDataModel = getRootMetaDataInput.ItemDataModel;

            var moduleId = itemDataModel.Id;

            var moduleFree = (FreeBusinessModel) itemDataModel.Data;

            var title = UrlHelper.NormalizeTextForUrl(FreeBusinessModule.GetTitle(moduleFree.Elements));

            var routaDatas = new Dictionary<string, string>();
            routaDatas.Add("action", "Index");
            routaDatas.Add("controller", ModuleName);
            routaDatas.Add("title", title);
            routaDatas.Add("moduleId", moduleId.ToString(CultureInfo.InvariantCulture));

            return routaDatas;
        }

    }
}