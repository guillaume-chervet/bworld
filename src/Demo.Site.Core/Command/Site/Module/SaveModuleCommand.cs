using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Mock;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Module
{
    public class SaveModuleCommand : Command<UserInput<Item>, CommandResult<string>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveModuleCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            var item = Input.Data;
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(item.SiteId, item.Id, false, true);

            MapItemToItemDataModel(item, itemDataModel);
            await _dataFactory.SaveChangeAsync();
            var siteId = item.SiteId;
            if (string.IsNullOrEmpty(siteId))
            {
                siteId = item.Id;
            }
            await _cacheProvider.UpdateCacheAsync(siteId);
        }
        public static void MapItemToItemDataModel(Item item, ItemDataModel itemDataModel)
        {
            itemDataModel.Index = item.Index;
            itemDataModel.IsTemporary = item.IsTemporary;
            itemDataModel.Data = MemoryDatabase.GetItemData(item);
            itemDataModel.Module = item.Module;
            itemDataModel.ParentId = item.ParentId;
            itemDataModel.PropertyName = item.PropertyName;
            itemDataModel.SiteId = item.SiteId;
            itemDataModel.Id = item.Id;
            itemDataModel.State = item.State;
        }
    }
}