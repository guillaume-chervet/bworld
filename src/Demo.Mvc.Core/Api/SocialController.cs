using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command;
using Demo.Mvc.Core.Sites.Core.Command.Social;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class SocialController : ApiControllerBase
    {

        public SocialController(BusinessFactory business)
            : base(business)
        {
        }

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [Route("api/social/get/{siteId}/{moduleId}")]
        public async Task<CommandResult> Get([FromServices]GetSocialCommand _getContactCommand, string siteId, string moduleId)
        {
            var result =
                await Business.InvokeAsync<GetSocialCommand, GetModuleInput, CommandResult<GetSocialResult>>(
                    _getContactCommand, new GetModuleInput {ModuleId = moduleId, SiteId = siteId});
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/social/save")]
        public async Task<CommandResult> Save([FromServices]SaveSocialCommand _saveContactCommand, [FromBody] SaveSocialInput updateFreeInput)
        {
            var userInput = new UserInput<SaveSocialInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            var result = await
                Business.InvokeAsync<SaveSocialCommand, UserInput<SaveSocialInput>, CommandResult<dynamic>>(
                    _saveContactCommand, userInput);

            return result;
        }
    }
}