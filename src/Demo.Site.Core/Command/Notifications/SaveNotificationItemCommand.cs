using System;
using System.Threading.Tasks;
using Demo.Business.Command.Free;
using Demo.Business.Command.News;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Notifications
{
    public class SaveNotificationItemCommand : Command<UserInput<SaveNewsItemInput>, CommandResult<dynamic>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;
        private readonly CacheProvider _cacheProvider;

        public SaveNotificationItemCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.Site.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var itemDataModel = await SaveFreeCommand.SaveItemDataModelAsync<NotificationItemBusinessModel>(_dataFactory, Input.Data, Input.UserId, NotificationItemBusinessModule.ModuleName);

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
                NewsItem = newsItemResult
            };
        }

    }
}