using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Command.Free;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Master
{
    public class SaveMasterCommand : Command<UserInput<SaveMasterInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveMasterCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
                await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, Input.Data.Site.SiteId);


            var itemDataModel = await GetMasterItemDataModelAsync(_dataFactory, Input.Data.Site.SiteId, true);
            if (itemDataModel == null)
            {
                itemDataModel = new ItemDataModel();
                itemDataModel.SiteId = Input.Data.Site.SiteId;
                itemDataModel.ParentId = Input.Data.Site.SiteId;
                itemDataModel.PropertyName = "Master";
                itemDataModel.Module = "Master";
                itemDataModel.Index = 100; // TODO
                _dataFactory.Add(itemDataModel);
            }

            var elements =
                await SaveFreeCommand.GetElementsAsync(_dataFactory, itemDataModel, Input.Data.Elements);

            // On enregistre l'element
            var freeBusinessModel = new MasterBusinessModel();
            freeBusinessModel.Elements = elements;
            itemDataModel.Data = freeBusinessModel;

            await _dataFactory.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(Input.Data.Site.SiteId);

            Result.Data = await _moduleManager.GetMasterAsync(Input.Data.Site);
        }

        public static async Task<ItemDataModel> GetMasterItemDataModelAsync(IDataFactory datafactory, string siteId,
            bool hasTracking = false)
        {
            var itemsDb = await datafactory.ItemRepository.GetItemsAsync(siteId, new ItemFilters
            {
                Module = "Master",
                ParentId = siteId,
                HasTracking = hasTracking,
                Limit = 1
            });

            return itemsDb.FirstOrDefault();
        }
    }
}