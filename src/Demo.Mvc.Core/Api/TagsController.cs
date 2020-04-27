using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command;
using Demo.Business.Command.Tags;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class TagsController : ApiControllerBase
    {

        public TagsController(BusinessFactory business)
            : base(business)
        {
        }


        [HttpGet]
        [ResponseCache(Duration = 0)]
        [Route("api/tags/{siteId}/{type}")]
        public async Task<CommandResult<GetTagsResult>> GetTags([FromServices]GetTagsCommand _getTagsCommand, string siteId, string type)
        {
            var userInput = new UserInput<GetTagsInput>
            {
                UserId = User.GetUserId(),
                Data = new GetTagsInput
                {
                    SiteId = siteId,
                    Type = type
                }
            };

            var result =
                await Business.InvokeAsync<GetTagsCommand, UserInput<GetTagsInput>, CommandResult<GetTagsResult>>(
                    _getTagsCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("api/tags")]
        public async Task<CommandResult<dynamic>> Save([FromServices]SaveTagsCommand _saveTagsCommand, [FromBody] SaveTagsInput saveUserInput)
        {
            var userInput = new UserInput<SaveTagsInput>
            {
                UserId = User.GetUserId(),
                Data = saveUserInput
            };

            var result = await
                Business.InvokeAsync<SaveTagsCommand, UserInput<SaveTagsInput>, CommandResult<dynamic>>(
                    _saveTagsCommand, userInput);

            return result;
        }
    }
}