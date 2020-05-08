using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command;
using Demo.Mvc.Core.Sites.Core.Command.News;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;
using Demo.Mvc.Core.Sites.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class NewsController : ApiControllerBase
    {

        public NewsController(BusinessFactory business)
            : base(business)
        {
   
        }


        [HttpGet]
        [Route("api/articles/get/{siteId}/{moduleId}")]
        public async Task<CommandResult> Get( [FromServices] GetNewsCommand _getNewsCommand, string siteId, string moduleId, IList<ItemState> states = null,
            int? index = null, string[] tags = null)
        {
            var input = new UserInput<GetNewsInput>
            {
                Data = new GetNewsInput
                {
                    ModuleId = moduleId,
                    SiteId = siteId,
                    FilterIndex = index,
                    States = states,
                    Tags = tags == null
                        ? null
                        : new List<string>(tags.Where(t => t != null && t != "null" && !string.IsNullOrEmpty(t)))
                },
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty
            };

            var result = await Business
                .InvokeAsync<GetNewsCommand, UserInput<GetNewsInput>, CommandResult<GetNewsResult>>(_getNewsCommand,
                    input).ConfigureAwait(false);
            return result;
        }

        [HttpGet]
        [Route("api/articles/item/get/{siteId}/{moduleId}")]
        public async Task<CommandResult> GetItem( [FromServices] GetNewsItemCommand _getNewsItemCommand, string siteId, string moduleId)
        {
            var userInput = new UserInput<GetModuleInput>
            {
                UserId = User.Identity == null ? string.Empty : User.GetUserId(),
                Data = new GetModuleInput {ModuleId = moduleId, SiteId = siteId}
            };

            var result = await Business
                .InvokeAsync<GetNewsItemCommand, UserInput<GetModuleInput>, CommandResult<GetNewsItemResult>>(
                    _getNewsItemCommand, userInput).ConfigureAwait(false);
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/articles/save")]
        public async Task<CommandResult> Save([FromServices] SaveNewsCommand _saveNewsCommand, [FromBody] SaveNewsInput updateFreeInput)
        {
            var userInput = new UserInput<SaveNewsInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            var result = await
                Business
                    .InvokeAsync<SaveNewsCommand, UserInput<SaveNewsInput>, CommandResult<dynamic>>(_saveNewsCommand,
                        userInput).ConfigureAwait(false);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/articles/item/save")]
        public async Task<CommandResult> SaveItem([FromServices] SaveNewsItemCommand _saveNewsItemCommand, [FromBody] SaveNewsItemInput updateFreeInput)
        {
            var userInput = new UserInput<SaveNewsItemInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            var result = await
                Business
                    .InvokeAsync<SaveNewsItemCommand, UserInput<SaveNewsItemInput>, CommandResult<dynamic>>(
                        _saveNewsItemCommand, userInput).ConfigureAwait(false);

            return result;
        }
    }
}