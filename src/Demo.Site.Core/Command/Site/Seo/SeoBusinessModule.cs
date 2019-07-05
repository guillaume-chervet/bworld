using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Data;
using Demo.Data.Model;
using Demo.Routing.Implementation;

namespace Demo.Business.Command.Site.Seo
{
    public class SeoBusinessModule : BusinessModuleBase
    {
        public SeoBusinessModule(BusinessModuleFactory businessModuleFactory) : base(businessModuleFactory)
        {
        }

        public override async Task GetInfoAsync(GeMenuItemInput geMenuItemInput)
        {
            var itemDataModel = geMenuItemInput.ItemDataModel;
            var seoBusinessModel = (SeoBusinessModel) geMenuItemInput.ItemDataModel.Data;

            ModuleManager.Add(geMenuItemInput.Master, itemDataModel.PropertyName, seoBusinessModel,
                itemDataModel.PropertyType);
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


            return itemDataModelDestination;
        }
    }
}