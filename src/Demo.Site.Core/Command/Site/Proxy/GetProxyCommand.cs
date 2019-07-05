using System.Collections.Generic;
using Demo.Business.Routing;
using Demo.Common.Command;
using Demo.Data;
using Demo.Routing.Interfaces;

namespace Demo.Business.Command.Site.Proxy
{
    public class GetProxyCommand : Command<string, CommandResult<GetProxyResult>>
    {
        private readonly IRouteProvider _routeProvider;

        public GetProxyCommand(IRouteProvider routeProvider)
        {
            _routeProvider = routeProvider;
        }

        protected override void Action()
        {
            var getProxyResult = new GetProxyResult();

            getProxyResult.Domains = new Dictionary<string, string>();

           var domains = _routeProvider.Domains;
            foreach (var domain in domains)
            {
                if (!string.IsNullOrEmpty(domain.XDomainRegex))
                {
                    if (!getProxyResult.Domains.ContainsKey(domain.XDomainRegex))
                    {
                        getProxyResult.Domains.Add(domain.XDomainRegex, "*");
                    }
                }
            }

            Result.Data = getProxyResult;
        }
    }
}