using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command;
using Demo.Business.Command.Administration.User;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class AdministrationUserController : ApiControllerBase
    {


        public AdministrationUserController(BusinessFactory business)
            : base(business)
        {
        }


        [Authorize]
        [HttpGet]
        [Route("api/admin/user/list/{siteId}")]
        public async Task<CommandResult> List([FromServices]ListUserCommand _listUserCommand, string siteId)
        {
            var userInput = new UserInput<string>
            {
                UserId = User.GetUserId(),
                Data = siteId
            };

            var result = await
                Business
                    .InvokeAsync<ListUserCommand, UserInput<string>, CommandResult<ListUserResult>>(_listUserCommand,
                        userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/user/{siteId}/{siteUserId}")]
        public async Task<CommandResult> Load([FromServices]LoadUserCommand _loadUserCommand, string siteId, string siteUserId)
        {
            var userInput = new UserInput<LoadUserInput>
            {
                UserId = User.GetUserId(),
                Data = new LoadUserInput
                {
                    SiteId = siteId,
                    SiteUserId = siteUserId
                }
            };

            var result = await
                Business
                    .InvokeAsync<LoadUserCommand, UserInput<LoadUserInput>, CommandResult<dynamic>>(_loadUserCommand,
                        userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/admin/user/save")]
        public async Task<CommandResult<SaveSiteUserOutput>> Save([FromServices]SaveSiteUserCommand _saveSiteUserCommand, [FromBody] SaveSiteUserInput saveSiteUserInput)
        {
            var userInput = new UserInput<SaveSiteUserInput>
            {
                UserId = User.GetUserId(),
                Data = saveSiteUserInput
            };

            var result = await
                Business
                    .InvokeAsync<SaveSiteUserCommand, UserInput<SaveSiteUserInput>, CommandResult<SaveSiteUserOutput>>(
                        _saveSiteUserCommand, userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/admin/user/sendInvitation")]
        public async Task<CommandResult> SendInvitation([FromServices]SendInvitationMailCommand _sendInvitationMailCommand,[FromBody] SendInvitationMailInput saveUserInput)
        {
            var userInput = new UserInput<SendInvitationMailInput>
            {
                UserId = User.GetUserId(),
                Data = saveUserInput
            };

            var result = await
                Business
                    .InvokeAsync<SendInvitationMailCommand, UserInput<SendInvitationMailInput>, CommandResult>(
                        _sendInvitationMailCommand, userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/admin/user/remove")]
        public async Task<CommandResult> Remove([FromServices]RemoveUserCommand _removeUserCommand, [FromBody] RemoveUserInput removeUserInput)
        {
            var userInput = new UserInput<RemoveUserInput>
            {
                UserId = User.GetUserId(),
                Data = removeUserInput
            };

            var result = await
                Business.InvokeAsync<RemoveUserCommand, UserInput<RemoveUserInput>, CommandResult>(_removeUserCommand,
                    userInput).ConfigureAwait(false);

            return result;
        }
    }
}