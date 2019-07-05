using Demo.Business;
using Demo.Business.Command.Site.Proxy;
using Demo.Common.Command;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Mvc.Core.Controllers
{
    public class ProxyController : ControllerBase
    {
        private readonly GetProxyCommand _getProxyCommand;
        //
        // GET: /Home/

        public ProxyController(BusinessFactory business, GetProxyCommand getProxyCommand)
            : base(business)
        {
            _getProxyCommand = getProxyCommand;
        }

        public ActionResult Index()
        {
            var result =
                Business.Invoke<GetProxyCommand, string, CommandResult<GetProxyResult>>(_getProxyCommand, null);

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            dynamic domainsJson =
                JsonConvert.SerializeObject(result.Data.Domains, Formatting.Indented, jsonSerializerSettings);
            ViewBag.DomainsJson = domainsJson;

            return View();
        }
    }
}