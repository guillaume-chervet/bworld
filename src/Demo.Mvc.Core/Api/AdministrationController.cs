using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Business.Command.Administration;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class AdministrationController : ApiControllerBase
    {

        public AdministrationController(BusinessFactory business)
            : base(business)
        {
        }

        [Authorize]
        [HttpGet]
        [Route("api/admin/get/{siteId}")]
        public async Task<CommandResult> Add([FromServices]GetAdministrationCommand _getAdministrationCommand, string siteId)
        {
            var userInput = new UserInput<string>
            {
                UserId = User.GetUserId(),
                Data = siteId
            };


            var result = await
                Business.InvokeAsync<GetAdministrationCommand, UserInput<string>, CommandResult<AdministrationModel>>(
                    _getAdministrationCommand, userInput);

            return result;
        }
    }
}