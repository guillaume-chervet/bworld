using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Menu
{
    public class SaveMenuCommand : Command<UserInput<SaveMenuInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveMenuCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager)
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

            var siteId = Input.Data.Site.SiteId;

            await UpdateMenuItemsAsync(siteId, Input.Data.MenuItems, "MenuItems");
            await UpdateMenuItemsAsync(siteId, Input.Data.PrivateMenuItems, "PrivateMenuItems");
            await UpdateMenuItemsAsync(siteId, Input.Data.BottomMenuItems, "BottomMenuItems");

            await _dataFactory.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(Input.Data.Site.SiteId);

            Result.Data = await _moduleManager.GetMasterAsync(Input.Data.Site);
        }

        private async Task UpdateMenuItemsAsync(string siteId, IList<MenuItem> menuItems, string propertyName)
        {
            if (menuItems != null)
            {
                var index = 0;
                foreach (var menuItem in menuItems)
                {

                    var parentId = menuItem.ParentId;
                    if (string.IsNullOrEmpty(parentId))
                    {
                        parentId = siteId;
                    }

                    var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(siteId, menuItem.ModuleId, false, true);
                    itemDataModel.Index = index;
                    itemDataModel.ParentId = parentId;
                    itemDataModel.PropertyName = propertyName;

                    index++;

                    if (menuItem.Childs != null)
                    {
                        await UpdateMenuItemsAsync(siteId, menuItem.Childs, propertyName);
                    }
                    
                }
            }
        }
    }
}