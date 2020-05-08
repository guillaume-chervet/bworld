using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command.Site
{
    public class SiteBusinessModule : IBusinessModuleCreate
    {
        private readonly BusinessModuleFactory _businessModuleFactory;

        public SiteBusinessModule(BusinessModuleFactory businessModuleFactory)
        {
            _businessModuleFactory = businessModuleFactory;
        }

        public async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            var itemDataModel = await DuplicateAllAsync( _businessModuleFactory,dataFactorySource, dataFactoryDestination, itemDataModelSource,
                itemDataModelDestinationParent, isTransfert, data);

            // Dans le cas d'une création de site depuis l'interface
            var addSiteInput = data as CreateFromSiteModel;
            if (addSiteInput != null)
            {
                var siteBusinessModel = (SiteBusinessModel) itemDataModel.Data;
                siteBusinessModel.Name = addSiteInput.SiteName;
                siteBusinessModel.CategoryId = addSiteInput.CategoryId;
                //siteBusinessModel.MasterDomainId = "Sites";
                siteBusinessModel.CultureId = "11";
            }

            return itemDataModel;
        }

        public async Task DeleteAsync(IDataFactory dataFactory, ICacheRepository cacheRepository, ItemDataModel itemDataModel)
        {
            await cacheRepository.DeleteAsync(itemDataModel.SiteId);
            await DeleteRecursiveAsync(_businessModuleFactory, dataFactory, cacheRepository, itemDataModel);
        }

        public static async Task DeleteRecursiveAsync(BusinessModuleFactory businessModuleFactory, IDataFactory dataFactory, ICacheRepository cacheRepository, ItemDataModel itemDataModel)
        {
            var siteId = itemDataModel.SiteId;
            if (string.IsNullOrEmpty(siteId))
            {
                siteId = itemDataModel.Id;
            }
            var items =
               await
                   dataFactory.ItemRepository.GetItemsAsync(siteId, new ItemFilters { ParentId = itemDataModel.Id });
            if (items != null)
            {
                foreach (var item in items)
                {
                    {
                        var module = businessModuleFactory.GetModuleCreate(item.Module);
                        if (module != null)
                        {
                            await module.DeleteAsync(dataFactory, cacheRepository, item);
                        }
                    }
                }
            }

            await dataFactory.DeleteAsync(itemDataModel);
        }

        public static async Task<ItemDataModel> DuplicateAllAsync( BusinessModuleFactory businessModuleFactory ,IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            var itemDataModelDestination = await AddSiteCommand.DuplicateItemAsync(dataFactoryDestination,
                itemDataModelSource,
                itemDataModelDestinationParent, isTransfert, data);

            var siteId = itemDataModelSource.SiteId;
            if (string.IsNullOrEmpty(siteId))
            {
                siteId = itemDataModelSource.Id;
            }

            var items =
                await
                    dataFactorySource.ItemRepository.GetItemsAsync(siteId,
                        new ItemFilters {ParentId = itemDataModelSource.Id});
            if (items != null)
            {
                var allLists = items.OrderBy(i => i.Index);
                foreach (var item in allLists)
                {
                    if (!item.IsTemporary)
                    {
                        var module = businessModuleFactory.GetModuleCreate(item.Module);
                        if (module != null)
                        {
                            await
                                module.CreateFromAsync(dataFactorySource, dataFactoryDestination, item,
                                    itemDataModelDestination,
                                    isTransfert, data);
                        }
                    }
                }
            }
            return itemDataModelDestination;
        }
    }
}