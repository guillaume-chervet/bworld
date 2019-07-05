using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command.Free.Models;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Identity.Models;
using Demo.Business.Command.News;

namespace Demo.Business.Command.Free
{
    public class GetFreeCommand : Command<UserInput<GetModuleInput>, CommandResult<GetFreeResult>>
    {
        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;

        public GetFreeCommand(IDataFactory dataFactory, UserService userService)
        {
            _userService = userService;
            _dataFactory = dataFactory;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId);

            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

            await GetNewsItemCommand.CheckAuthorisationAsync(_userService, itemDataModel, Input.UserId);

            Result.Data =  await GetNewsItemCommand.GetFreeResultAsync<FreeBusinessModel, GetFreeResult>( _userService, itemDataModel, _dataFactory.ItemRepository);
        }

        public static async Task<UserInfoResult> GetUserInfoAsync<T>(UserService userService, T moduleNews) where T : FreeBusinessModel
        {
            if (moduleNews == null)
            {
                return null;
            }
            var userInfo = await userService.GetUserInfoAsync(moduleNews.AuthorUserId);

            var userInfoResult = new UserInfoResult();

            UserInfo lastUpdateUserInfo = null;
            if (!string.IsNullOrEmpty(moduleNews.LastUpdateAuthorUserId))
            {
                if (moduleNews.AuthorUserId != moduleNews.LastUpdateAuthorUserId)
                {
                    lastUpdateUserInfo = await userService.GetUserInfoAsync(moduleNews.LastUpdateAuthorUserId);
                }
                else
                {
                    lastUpdateUserInfo = userInfo;
                }
            }

            userInfoResult.LastUpdateUserInfo = lastUpdateUserInfo;
            userInfoResult.UserInfo = userInfo;

            return userInfoResult;
        }
    }
}