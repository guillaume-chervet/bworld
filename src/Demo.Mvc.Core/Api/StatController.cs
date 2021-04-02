using System.Security.Claims;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class StatController : ApiControllerBase
    {

        public StatController(BusinessFactory business)
            : base(business)
        {
        }


        [Authorize]
        [HttpPost]
        [ResponseCache(Duration = 0)]
        [Route("api/stat/get")]
        public async Task<CommandResult> Get([FromServices]GetStatsCommand _getStatsCommand, [FromBody] GetStatsInput data)
        {
            var userInput = new UserInput<GetStatsInput>
            {
                UserId = User.GetUserId(),
                Data = data
            };

            var result = await Business
                .InvokeAsync<GetStatsCommand, UserInput<GetStatsInput>, CommandResult<GetStatsResult>>(_getStatsCommand,
                    userInput).ConfigureAwait(false);

            return result;
        }


        [HttpPost]
        [Route("api/stat/save")]
        public async Task<CommandResult<SaveStatsResults>> Save([FromServices]SaveStatsCommand _saveStatsCommand, [FromBody] SaveStatsClientInput data)
        {
            var saveStatsInput = new SaveStatsInput();
            var userId = GetUserId(User);

            saveStatsInput.UserId = userId;
            saveStatsInput.IpAdress = HttpContext.Connection.RemoteIpAddress.ToString();
            saveStatsInput.UserAgent = Request.Headers["User-Agent"].ToString();
            saveStatsInput.Client = data;

            var result = await Business
                .InvokeAsync<SaveStatsCommand, SaveStatsInput, CommandResult<SaveStatsResults>>(_saveStatsCommand,
                    saveStatsInput).ConfigureAwait(false);

            return result;
        }

        public static string GetUserId(ClaimsPrincipal user)
        {
            var userId = string.Empty;
            if (user.Identity.IsAuthenticated) userId = user.Identity.IsAuthenticated ? user.GetUserId() : string.Empty;
            return userId;
        }
    }
}