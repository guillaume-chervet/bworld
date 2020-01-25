using System;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;

namespace Demo.Business.Command.Site
{
    /// <summary>
    ///     Command qui permet de copier un site d'une source de données vers une autre
    /// </summary>
    public class TransfertSiteCommand : Command<TransfertSiteInput, CommandResult>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly BusinessModuleFactory _businessModuleFactory;
        private readonly IDataFactory _dataFactorySource;
        private readonly IDataFactory _dataFactoryDestination;

        public TransfertSiteCommand(IDataFactory dataFactorySource, IDataFactory dataFactoryDestination,
            CacheProvider cacheProvider, BusinessModuleFactory businessModuleFactory)
        {
            _dataFactorySource = dataFactorySource;
            _dataFactoryDestination = dataFactoryDestination;
            _cacheProvider = cacheProvider;
            _businessModuleFactory = businessModuleFactory;
        }

        protected override async Task ActionAsync()
        {
            #region duplication du site

            var siteId = Input.SiteId;
            if (!string.IsNullOrEmpty(siteId))
            {
                await DuplicateSiteAsync(siteId);
            }
            else
            {
                var siteDataModels = await _dataFactorySource.ItemRepository.GetItemsAsync(null, new ItemFilters {Module = "Site"});

                foreach (var siteDataModel in siteDataModels)
                {
                    await DuplicateSiteAsync(siteDataModel.Id);
                }
            }

            #endregion
        }

        private async Task DuplicateSiteAsync(string siteId)
        {

            var itemDataModel = await GetSiteAsync(siteId, _dataFactorySource);
            var moduleSite = _businessModuleFactory.GetModuleCreate(itemDataModel.Module);
            var siteItemDataModel = await moduleSite.CreateFromAsync(_dataFactorySource, _dataFactoryDestination, itemDataModel, null,
                true, null);
            await _dataFactoryDestination.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(siteItemDataModel.Id);
        }

        public static async Task<ItemDataModel> GetSiteAsync(string universaleSiteId, IDataFactory dataFactorySource)
        {
            var itemRepository = dataFactorySource.ItemRepository;
            return await itemRepository.GetItemAsync(null, universaleSiteId);
        }
        
    }
}