using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Geo.Core;
using Demo.Mvc.Core.Sites.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    public class GeroController : ApiControllerBase
    {

        public GeroController(BusinessFactory business)
            : base(business)
        {
        }

        [Authorize]
        [HttpPost]
        [Route("api/geo/post")]
        public async Task<CommandResult> Get([FromServices] GetGeoCommand geoCommand, [FromBody] GetGeoInput address)
        {
            var result =
                await Business.InvokeAsync<GetGeoCommand, GetGeoInput, CommandResult<dynamic>>(geoCommand, address);
            return result;
        }

        /*  [Authorize]
       
        [HttpPost]
        [Route("api/geo/getfromip")]
        public CommandResult Get()
        {
            var result = Business.Invoke<GetGeoCommand, GetGeoInput, CommandResult<dynamic>>(address);
            return result;
        }

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }*/
    }
}