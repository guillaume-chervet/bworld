using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command
{
    public abstract class BusinessModuleBase : IBusinessModule, IBusinessModuleCreate
    {
        protected readonly BusinessModuleFactory _businessModuleFactory;

        public BusinessModuleBase(BusinessModuleFactory businessModuleFactory)
        {
            _businessModuleFactory = businessModuleFactory;
        }

        public abstract Task GetInfoAsync(GeMenuItemInput geMenuItemInput);
        public abstract IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput);
        public abstract IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas);

        public abstract Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource, IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource, ItemDataModel itemDataModelDestinationParent, bool isTransfert,
            object data = null);

        public async Task DeleteAsync(IDataFactory dataFactory, ICacheRepository cacheRepository, ItemDataModel itemDataModel)
        {
            await SiteBusinessModule.DeleteRecursiveAsync(_businessModuleFactory, dataFactory, cacheRepository, itemDataModel);
        }
    }
}
