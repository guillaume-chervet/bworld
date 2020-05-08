using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command.User;
using Demo.Mvc.Core.UserCore;
using Demo.User.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class UserController : ApiControllerBase
    {

        public UserController(BusinessFactory business)
            : base(business)
        {
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/getusers")]
        public async Task<CommandResult> GetUsers([FromServices]GetUsersCommand _getUsersCommand)
        {
            var userInput = new UserInput<string>
            {
                Data = string.Empty,
                UserId = User.GetUserId()
            };

            var result =
                await Business
                    .InvokeAsync<GetUsersCommand, UserInput<string>, CommandResult<IList<User.User>>>(_getUsersCommand,
                        userInput).ConfigureAwait(false);

            return result;
        }


        [HttpGet]
        [Route("api/user/getinfo/{siteId}")]
        public async Task<CommandResult<GetUserInfoResult>> GetInfo([FromServices]GetUserInfoCommand _getUserInfoCommand, string siteId)
        {
            var userInput = new UserInput<string>
            {
                Data = siteId,
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty
            };

            var result =
                await Business
                    .InvokeAsync<GetUserInfoCommand, UserInput<string>, CommandResult<GetUserInfoResult>>(
                        _getUserInfoCommand, userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/notification/{siteId}")]
        public async Task<CommandResult<GetNotificationResult>> GetNotification([FromServices]GetNotificationCommand _getNotificationCommand, string siteId)
        {
            var userInput = new UserInput<string>
            {
                Data = siteId,
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty
            };

            var result =
                await Business
                    .InvokeAsync<GetNotificationCommand, UserInput<string>, CommandResult<GetNotificationResult>>(
                        _getNotificationCommand, userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/user/getuser")]
        public async Task<CommandResult> GetUser([FromServices]GetUserCommand _getUserCommand)
        {
            var userInput = new UserInput<string>
            {
                Data = string.Empty,
                UserId = User.GetUserId()
            };

            var result = await
                Business.InvokeAsync<GetUserCommand, UserInput<string>, CommandResult<GetUserResult>>(_getUserCommand,
                    userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpDelete]
        [Route("api/user/delete/{userId}")]
        public async Task<CommandResult> DeleteUser([FromServices]DeleteUserCommand _deleteUserCommand, string userId)
        {
            var userInput = new UserInput<string>
            {
                Data = userId,
                UserId = User.GetUserId()
            };


            var result = await Business
                .InvokeAsync<DeleteUserCommand, UserInput<string>, CommandResult>(_deleteUserCommand, userInput)
                .ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/save")]
        public async Task<CommandResult> SaveUser([FromServices]SaveUserCommand _saveUserCommand,[FromBody] SaveUserInput saveUser)
        {
            var userInput = new UserInput<SaveUserInput>
            {
                Data = saveUser,
                UserId = User.GetUserId()
            };

            var result = await Business
                .InvokeAsync<SaveUserCommand, UserInput<SaveUserInput>, CommandResult>(_saveUserCommand, userInput)
                .ConfigureAwait(false);

            return result;
        }

        [HttpGet]
        [Authorize]
        [Route("api/user/logoff")]
        public async Task<CommandResult> LogOff([FromServices] SignInManager<ApplicationUser> signInManager)
        {
            var result = new CommandResult();
            await signInManager.SignOutAsync();
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/user/deleteuserlogin")]
        public async Task<CommandResult> DeleteUserLoginCommand([FromServices]DeleteUserLoginCommand _deleteUserLoginCommand, [FromBody] DeleteUserLoginInput saveUser)
        {
            var userInput = new UserInput<DeleteUserLoginInput>
            {
                Data = saveUser,
                UserId = User.GetUserId()
            };

            var result = await Business
                .InvokeAsync<DeleteUserLoginCommand, UserInput<DeleteUserLoginInput>, CommandResult>(
                    _deleteUserLoginCommand, userInput).ConfigureAwait(false);

            return result;
        }
    }
}