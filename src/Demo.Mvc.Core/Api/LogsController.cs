using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Log;
using Demo.Log.Core;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class LogsController : ApiControllerBase
    {

        public LogsController(BusinessFactory business)
            : base(business)
        {
        }

        [Authorize]
        [HttpPost]
        [Route("api/logs/filter")]
        public async Task<CommandResult> Get([FromServices] GetLogCommand _getLogCommand, [FromBody] GetLogsInput getLogsInput)
        {
            var userInput = new UserInput<GetLogsInput>
            {
                Data = getLogsInput,
                UserId = User.GetUserId()
            };

            var result =
                await Business.InvokeAsync<GetLogCommand, UserInput<GetLogsInput>, CommandResult<dynamic>>(
                    _getLogCommand, userInput);
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/logs/clear")]
        public async Task<CommandResult> Clear([FromServices] ClearLogCommand _clearLogCommand)
        {
            var userInput = new UserInput<string>
            {
                Data = string.Empty,
                UserId = User.GetUserId()
            };

            var result =
                await Business.InvokeAsync<ClearLogCommand, UserInput<string>, CommandResult<dynamic>>(_clearLogCommand,
                    userInput);
            return result;
        }
    }
}