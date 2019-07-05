using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Business.Command.Site;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.Routing.Implementation;

namespace Demo.Business.Command
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
