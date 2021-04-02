using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class FreeController : ApiControllerBase
    {

        public FreeController(BusinessFactory business)
            : base(business)
        {
        }

        [ResponseCache(Duration = 0)]
        [HttpGet]
        [Route("api/free/get/{siteId}/{moduleId}")]
        public async Task<CommandResult> Get([FromServices] GetFreeCommand _getFreeCommand, string siteId, string moduleId)
        {
            var input = new UserInput<GetModuleInput>
            {
                Data = new GetModuleInput {ModuleId = moduleId, SiteId = siteId},
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty
            };


            var result = await
                Business
                    .InvokeAsync<GetFreeCommand, UserInput<GetModuleInput>, CommandResult<GetFreeResult>>(
                        _getFreeCommand, input).ConfigureAwait(false);
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/free/save")]
        public async Task<CommandResult> Save([FromServices] SaveFreeCommand _saveFreeCommand, [FromBody] SaveFreeInput updateFreeInput)
        {
            var userInput = new UserInput<SaveFreeInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            var result = await Business
                .InvokeAsync<SaveFreeCommand, UserInput<SaveFreeInput>, CommandResult<dynamic>>(_saveFreeCommand,
                    userInput).ConfigureAwait(false);
            ;

            return result;
        }
    }
}