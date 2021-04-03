using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Command.News;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Core.Models.Page;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Core.Command.Free
{
    public class FreeBusinessModule : BusinessModuleBase
    {
        private readonly UrlProvider _urlProvider;
        public const string Url = "page/{moduleid}/{title}";
        public const string UrlPrivate = "{private}/page/{moduleid}/{title}";
        public const string ModuleName = "Free";

        #region Liste des route associé au module

        public FreeBusinessModule(BusinessModuleFactory businessModuleFactory, UrlProvider urlProvider) : base(businessModuleFactory)
        {
            _urlProvider = urlProvider;
        }

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new[]
            {
                  new Route
                {
                    Identity = "1",
                    Action = "Index",
                    Controller = "Free",
                    Path = UrlPrivate,
                    Regex = @"^(.+)\/page\/.+\/.+$",
                    RewritePath = @"{private}\{title}"
                },
                new Route
                {
                    Identity = "2",
                    Action = "Index",
                    Controller = "Free",
                    Path = Url,
                    Regex = @"^page\/.+\/.+$",
                    RewritePath = "{title}"
                },
                new Route
                {
                    Identity = "3",
                    Action = "Admin",
                    Controller = "Free",
                    Path = "administration/page/{moduleid}/{title}",
                    Regex = @"^administration\/page\/.+\/.+$"
                }
            };
        }

        #endregion

        public override async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            // TODO attention les id des images ne sont pas mis à jour
            return
                await
                    SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory,dataFactorySource, dataFactoryDestination, itemDataModelSource,
                        itemDataModelDestinationParent, isTransfert, data);
        }

        #region Génération du menu associé au module

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var currentRequest = geMenuItemInput.CurrentRequest;
            var propertyName = itemDataModel.PropertyName;

            var isPrivate = NewsItemBusinessModule.IsPrivate(propertyName);

            if (isPrivate && geMenuItemInput.IsSitemap)
            {
                return;
            }

            var menuItem = await NewsItemBusinessModule.GetMenuItemAsync(_urlProvider,itemDataModel,  currentRequest, ModuleName, isPrivate);
            if (menuItem != null)
            {
                //var expendoMenu = CacheProvider.ToExpando(menuItem);
                {
                    var items =
                        await
                            geMenuItemInput.DataFactory.ItemRepository.GetItemsAsync(itemDataModel.SiteId,
                                new ItemFilters
                                {
                                    ParentId = itemDataModel.Id,
                                    ExcludedModules = new List<string>() {"Image", "Video"}
                                });
                    foreach (var dataModel in items)
                    {
                        var menuItemChild = await NewsItemBusinessModule.GetMenuItemAsync(_urlProvider, dataModel,
                            currentRequest, dataModel.Module, isPrivate);
                        if (menuItemChild != null)
                        {
                            if (menuItem.Childs == null)
                            {
                                menuItem.Childs = new List<MenuItemBusiness>();
                            }
                            menuItem.Childs.Add(menuItemChild);
                        }
                    }
                }

                ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, menuItem,
                    itemDataModel.PropertyType);
            }
        }

        public static string GetFirstImageUrl(string siteId, ElementBusinessModel freeBusinessModel)
        {
            foreach (var element in freeBusinessModel.Elements)
            {
                if (element.Type == "image" || element.Type == "file" || element.Type == "carousel")
                {
                    var fileDatas = JsonConvert.DeserializeObject<IList<FileData>>(element.Data);

                    if (fileDatas.Count > 0)
                    {
                        var fileData = fileDatas[0];
                        var url = string.Format(@"/api/file/get/{0}/{1}/{2}/{3}", siteId, fileData.Id, "ImageThumb",
                            UrlHelper.NormalizeTextForUrl(fileData.Name));
                        return url;
                    }
                }
            }

            return null;
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            var itemDataModel = getRootMetaDataInput.ItemDataModel;

            var moduleId = itemDataModel.Id;

            var moduleFree = (FreeBusinessModel) itemDataModel.Data;

            var title = UrlHelper.NormalizeTextForUrl(GetTitle(moduleFree.Elements));

            var routaDatas = new Dictionary<string, string>();
            routaDatas.Add("action", "Index");
            routaDatas.Add("controller", ModuleName);
            routaDatas.Add("title", title);
            routaDatas.Add("moduleId", moduleId.ToString(CultureInfo.InvariantCulture));

            return routaDatas;
        }

        public static void UpdateContent(IList<Element> elements, string type = null, string defaultContent = "Sans titre")
        {
            var titleElement = elements.FirstOrDefault(e => e.Type == "h1");

            if (titleElement != null && string.IsNullOrEmpty(titleElement.Data))
            {
                titleElement.Data = defaultContent;
            }

        }

        public static string GetTitle(IList<Element> elements, string type = null, string defaultContent = "Sans titre")
        {
            if (string.IsNullOrEmpty(type))
            {
                type = "h1";
            }
            var title = defaultContent;
            var titleElement = elements?.FirstOrDefault(e => e.Type == type);
           
            if (titleElement != null && !string.IsNullOrEmpty(titleElement.Data))
            {
                title = titleElement.Data;
            }
            return title;
        }

        public static string GetMetaDescription(IList<Element> elements)
        {
            var metaDescription = elements.FirstOrDefault(e => e.Property == "MetaDescription");
            if (metaDescription != null && !string.IsNullOrEmpty(metaDescription.Data))
            {
                return metaDescription.Data; //UrlHelper.NormalizeTextForHtml(metaDescription.Data);
            }
            var title = GetTitle(elements);
            var p = GetTitle(elements, "p", string.Empty);
            if (string.IsNullOrEmpty(p))
            {
                return title;//UrlHelper.NormalizeTextForHtml(title);
            }
            var meta = Regex.Replace(p, "<[^>]*>", string.Empty);
            return title + " : " + meta; //UrlHelper.NormalizeTextForHtml(title + " : " + meta);
        }

        #endregion
    }
}