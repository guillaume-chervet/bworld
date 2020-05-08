using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.UserCore.SiteData;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class UserDataController : ApiControllerBase
    {

        public UserDataController(BusinessFactory business)
            : base
                (business)
        {
        }


        [HttpGet]
        [Route("api/user/data/{siteId}")]
        public async Task<CommandResult<InputUserDataResult>> GetUsers([FromServices]GetUserDataCommand _getUserDataCommand, string siteId, string cookieSessionId = null)
        {
            var userInput = new UserInput<GetUserDataInput>
            {
                Data = new GetUserDataInput {SiteId = siteId},
                UserId = User?.GetUserId()
            };

            var result =
                await Business
                    .InvokeAsync<GetUserDataCommand, UserInput<GetUserDataInput>, CommandResult<InputUserDataResult>>(
                        _getUserDataCommand, userInput).ConfigureAwait(false);

            return result;
        }


        [HttpPost]
        [Route("api/user/data")]
        public async Task<CommandResult<string>> SaveUser([FromServices]SaveUserDataCommand _saveUserDataCommand, [FromBody] SaveUserDataInput saveUser)
        {
            var userInput = new UserInput<SaveUserDataInput>
            {
                Data = saveUser,
                UserId = User?.GetUserId()
            };

            var result = await Business
                .InvokeAsync<SaveUserDataCommand, UserInput<SaveUserDataInput>, CommandResult<string>>(
                    _saveUserDataCommand, userInput).ConfigureAwait(false);

            return result;
        }
    }
}