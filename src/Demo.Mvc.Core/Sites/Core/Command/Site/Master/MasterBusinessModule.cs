using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Common;
using Demo.Mvc.Core.Renderer;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Models.Page;
using Demo.Mvc.Core.Sites.Core.Renderers;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Master
{
    public class MasterBusinessModule : BusinessModuleBase
    {
        private readonly IRouteProvider _routeProvider;
        public const string ModuleName = "Master";

        public MasterBusinessModule(BusinessModuleFactory businessModuleFactory, IRouteProvider routeProvider) : base(businessModuleFactory)
        {
            _routeProvider = routeProvider;
        }

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var moduleFree = (MasterBusinessModel) geMenuItemInput.ItemDataModel.Data;

            var masterInfo = new Master();

            masterInfo.Id = itemDataModel.Id;

            masterInfo.ImageIcones = GetImages(moduleFree, "ImageIcone");
            if (masterInfo.ImageIcones != null && masterInfo.ImageIcones.Count > 0)
            {
                var imageIcone = masterInfo.ImageIcones.First();
                masterInfo.ImageIconeId = imageIcone.Id;
                masterInfo.ImageIconeFileName = imageIcone.Name;
            }
            masterInfo.ImageLogos = GetImages(moduleFree, "ImageLogo");
            if (masterInfo.ImageLogos != null && masterInfo.ImageLogos.Count > 0)
            {
                var imageLogo = masterInfo.ImageLogos.First();
                masterInfo.ImageLogoId = imageLogo.Id;
                masterInfo.ImageLogoFileName = imageLogo.Name;
            }

            var jumbotron = moduleFree.Elements.FirstOrDefault(e => e.Property == "Jumbotron");
            if (jumbotron != null)
            {
                masterInfo.IsJumbotron = jumbotron.Data == "true";
            }

            masterInfo.Title = moduleFree.Elements.First(e => e.Property == "Title").Data;
            var colorBackgroundMenu = GetData(moduleFree, "ColorBackgroundMenu");

            masterInfo.ColorBackgroundMenu = colorBackgroundMenu;
            masterInfo.ColorHoverBackgroundMenu = ColorHelper.Grayer(colorBackgroundMenu, 50);
            masterInfo.ColorSelectedBackgroundMenu = ColorHelper.Grayer(colorBackgroundMenu, 32);
            masterInfo.ColorH1 = GetData(moduleFree, "ColorH1");
            masterInfo.ColorH2 = GetData(moduleFree, "ColorH2");
            masterInfo.ColorH3 = GetData(moduleFree, "ColorH3");
            masterInfo.ColorJumbotron = GetData(moduleFree, "ColorJumbotron");
            masterInfo.ColorBackgroundMenuBottom = GetData(moduleFree, "ColorBackgroundMenuBottom");
            var color = GetData(moduleFree, "Color");
            masterInfo.Color = color;
            var colorBackground = GetData(moduleFree, "ColorBackground");
            masterInfo.ColorBackground = colorBackground;
            var colorH1 = GetData(moduleFree, "ColorH1");

            var styleModel = new StyleModel
            {
                Color =color,
                ColorBackground= colorBackground,
                ColorBackgroundMenu = colorBackgroundMenu,
                ColorHoverBackgroundMenu = ColorHelper.Grayer(colorBackgroundMenu, 50),
                ColorBackgroundTableHeader = ColorHelper.Grayer(colorBackgroundMenu, -20),
                ColorBackgroundTableFooter = ColorHelper.Grayer(colorBackgroundMenu, -40),
                ColorSelectedBackgroundMenu = ColorHelper.Grayer(colorBackgroundMenu, 32),
                ColorH1 = colorH1,
                ColorH2 = GetData(moduleFree, "ColorH2"),
                ColorJumbotron = GetData(moduleFree, "ColorJumbotron"),
                ColorBackgroundMenuBottom = GetData(moduleFree, "ColorBackgroundMenuBottom"),
                ColorLoader = ColorHelper.Grayer(colorBackgroundMenu, -20),
                ColorSeparator = ColorHelper.Grayer(colorBackgroundMenu, -70, 0, 220)
        };

            var template = ResourcesLoader.Load(Path.Combine("Sites", "Core", "Renderers", "Style.st"));
            var styleTemplate = new StringTemplateRenderer().Render(template, styleModel);

            masterInfo.StyleTemplate = styleTemplate;

            //var theme = moduleFree.Elements.FirstOrDefault(e => e.Property == "Theme");
            //TODO
            masterInfo.Theme = "theme1";
           /* if (theme != null)
            {
                masterInfo.Theme = theme.Data;
            }
            else
            {
                masterInfo.Theme = "default";
            }*/

            masterInfo.Seo = new SeoBusinessMaster();

            
            masterInfo.FacebookAuthenticationAppId =
                _routeProvider.Domains.Where(d => d.Id == geMenuItemInput.CurrentRequest.DomainId)
                    .Select(d => d.FacebookAppId)
                    .FirstOrDefault();

            ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, masterInfo, itemDataModel.PropertyType);
        }

        public override IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput)
        {
            return new Dictionary<string, string>();
        }

        public override IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            return new List<Route>();
        }

        public override async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            var itemDataModelDestination = await SiteBusinessModule.DuplicateAllAsync(_businessModuleFactory,dataFactorySource,
                dataFactoryDestination, itemDataModelSource, itemDataModelDestinationParent, isTransfert);

            var addSiteInput = data as CreateFromSiteModel;
            if (addSiteInput != null)
            {
                var siteBusinessModel = (MasterBusinessModel) itemDataModelDestination.Data;
                siteBusinessModel.Elements.First(e => e.Property == "Title").Data = addSiteInput.Title;

                // On met à jour les id des images
                var imageIcones = GetImages(siteBusinessModel, "ImageIcone");
                if (imageIcones != null && imageIcones.Count > 0)
                {
                    UpdateImageId(imageIcones, itemDataModelDestination);
                    var element = siteBusinessModel.Elements.First(e => e.Property == "ImageIcone");
                    element.Data = JsonConvert.SerializeObject(imageIcones);
                }
                var imageLogos = GetImages(siteBusinessModel, "ImageLogo");
                if (imageLogos != null && imageLogos.Count > 0)
                {
                    UpdateImageId(imageLogos, itemDataModelDestination);
                    var element = siteBusinessModel.Elements.First(e => e.Property == "ImageLogo");
                    element.Data = JsonConvert.SerializeObject(imageLogos);
                }
            }

            return itemDataModelDestination;
        }

        private static string GetData(MasterBusinessModel moduleFree, string property, string defaultData= null)
        {
            var element = moduleFree.Elements.FirstOrDefault(e => e.Property == property);
            string color = defaultData;
            if (element != null && ! string.IsNullOrEmpty(element.Data))
            {
                color = element.Data;
            }
            return color;
        }

        private static void UpdateImageId(IList<DataFileInput> imageIcones, ItemDataModel itemDataModelDestination)
        {
            foreach (var dataFileInput in imageIcones)
            {
                dataFileInput.Id =
                    itemDataModelDestination.Childs.Where(
                        c => c.Module == "Image" && c.PropertyName == dataFileInput.PropertyName)
                        .Select(c => c.Id).First();
            }
        }

        private IList<DataFileInput> GetImages(MasterBusinessModel moduleFree, string propertyName)
        {
            var element = moduleFree.Elements.FirstOrDefault(e => e.Property == propertyName);

            if (element == null || string.IsNullOrEmpty(element.Data))
                return null;

            return JsonConvert.DeserializeObject<List<DataFileInput>>(element.Data);
        }
    }
}