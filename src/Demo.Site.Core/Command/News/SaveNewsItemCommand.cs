using System;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Command.Free;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.News
{
    public class SaveNewsItemCommand : Command<UserInput<SaveNewsItemInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveNewsItemCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.Site.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var itemDataModel = await SaveFreeCommand.SaveItemDataModelAsync<NewsItemBusinessModel>(_dataFactory, Input.Data, Input.UserId, "NewsItem");
            var isNew = string.IsNullOrEmpty(itemDataModel.Id);
            await _dataFactory.SaveChangeAsync();

            await _cacheProvider.UpdateCacheAsync(siteId);

            GetNewsItemResult newsItemResult = null;

            if (isNew)
            {
                newsItemResult =
                    await
                        GetNewsItemCommand.GetNewsItemResult(_dataFactory, _userService, itemDataModel, Input.UserId);
            }
            Result.Data = new
            {
                Master = await _moduleManager.GetMasterAsync(Input.Data.Site),
                NewsItem = newsItemResult
            };
        }
    }
}