using System.Threading.Tasks;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;

namespace Demo.Business.BusinessModule
{
    public interface IBusinessModuleCreate
    {
        /// <summary>
        ///     Duplication du module
        /// </summary>
        /// <param name="dataFactoryDestination"></param>
        /// <param name="itemDataModelSource"></param>
        /// <param name="itemDataModelDestinationParent"></param>
        /// <param name="dataFactorySource"></param>
        /// <param name="isTransfert"></param>
        /// <param name="data"></param>
        Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource, IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource, ItemDataModel itemDataModelDestinationParent, bool isTransfert,
            object data = null);

        Task DeleteAsync( IDataFactory dataFactory, ICacheRepository cacheRepository, ItemDataModel itemDataModel);
    }
}