using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.News;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Notifications
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