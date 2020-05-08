using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command;
using Demo.Mvc.Core.Sites.Core.Command.Site.Menu;
using Demo.Mvc.Core.Sites.Core.Command.Site.Module;
using Demo.Mvc.Core.Sites.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class ModuleController : ApiControllerBase
    {

        public ModuleController(BusinessFactory business)
            : base(business)
        {
        }

        [Authorize]
        [HttpPost]
        [Route("api/module/delete")]
        public async Task<CommandResult> Remove([FromServices] DeleteModuleCommand _deleteModuleCommand, [FromBody] DeleteModuleInput deleteModuleInput)
        {
            var userInput = new UserInput<DeleteModuleInput>
            {
                UserId = User.GetUserId(),
                Data = deleteModuleInput
            };

            var result = await
                Business.InvokeAsync<DeleteModuleCommand, UserInput<DeleteModuleInput>, CommandResult<dynamic>>(
                    _deleteModuleCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpDelete]
        [Route("api/module/superadmindelete/{siteId}/{moduleId}")]
        public async Task<CommandResult> SuperAdminRemove([FromServices] SuperAdminDeleteModuleCommand _superAdminDeleteModuleCommand, string siteId, string moduleId)
        {
            var userInput = new UserInput<GetModuleInput>
            {
                UserId = User.GetUserId(),
                Data = new GetModuleInput {SiteId = siteId, ModuleId = moduleId}
            };

            var result = await
                Business.InvokeAsync<SuperAdminDeleteModuleCommand, UserInput<GetModuleInput>, CommandResult<string>>(
                    _superAdminDeleteModuleCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/module/superadminsave")]
        public async Task<CommandResult> SuperAdminSave([FromServices] SaveModuleCommand _saveModuleCommand, [FromBody] Item item)
        {
            var userInput = new UserInput<Item>
            {
                UserId = User.GetUserId(),
                Data = item
            };

            var result =
                await Business.InvokeAsync<SaveModuleCommand, UserInput<Item>, CommandResult<string>>(
                    _saveModuleCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/module/superadminget")]
        public async Task<CommandResult> SuperAdminGet([FromServices] GetModuleCommand _getModuleCommand,[FromBody] GetModuleInput getModuleInput)
        {
            var userInput = new UserInput<GetModuleInput>
            {
                UserId = User.GetUserId(),
                Data = getModuleInput
            };

            var result = await
                Business.InvokeAsync<GetModuleCommand, UserInput<GetModuleInput>, CommandResult<Item>>(
                    _getModuleCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/module/menu/save")]
        public async Task<CommandResult> Save([FromServices] SaveMenuCommand saveMenuCommand, [FromBody] SaveMenuInput saveMenuInput)
        {
            var userInput = new UserInput<SaveMenuInput>
            {
                UserId = User.GetUserId(),
                Data = saveMenuInput
            };

            var result = await
                Business.InvokeAsync<SaveMenuCommand, UserInput<SaveMenuInput>, CommandResult<dynamic>>(
                    saveMenuCommand, userInput);

            return result;
        }
    }
}