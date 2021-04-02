using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command.Site.Seo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class SeoController : ApiControllerBase
    {


        public SeoController(BusinessFactory business)
            : base(business)
        {
        }


        [Authorize]
        [HttpGet]
        [ResponseCache(Duration = 0)]
        [Route("api/seo/get/{siteId}")]
        public async Task<CommandResult> Get([FromServices]GetSeoCommand _getSeoCommand, string siteId)
        {
            var userInput = new UserInput<GetSeoInput>
            {
                UserId = User.GetUserId(),
                Data = new GetSeoInput {SiteId = siteId, IsVerifyAuthorisation = true}
            };

            var result =
                await Business.InvokeAsync<GetSeoCommand, UserInput<GetSeoInput>, CommandResult<SeoBusinessModel>>(
                    _getSeoCommand, userInput);

            return result;
        }


        [HttpPost]
        [Route("api/seo/save")]
        public async Task<CommandResult> Save( [FromServices] SaveSeoCommand _saveSeoCommand, [FromBody] SaveSeoInput data)
        {
            var userInput = new UserInput<SaveSeoInput>
            {
                Data = data,
                UserId = User.GetUserId()
            };

            var result =
                await Business.InvokeAsync<SaveSeoCommand, UserInput<SaveSeoInput>, CommandResult<dynamic>>(
                    _saveSeoCommand, userInput);

            return result;
        }
    }
}