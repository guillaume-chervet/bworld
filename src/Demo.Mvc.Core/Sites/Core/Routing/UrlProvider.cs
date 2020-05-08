using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Models;

namespace Demo.Mvc.Core.Sites.Core.Routing
{
    public class UrlProvider
    {
        private readonly IRouteManager _routeManager;

        public UrlProvider(IRouteManager routeManager)
        {
            _routeManager = routeManager;
        }


        public async Task<FindPathResult> GetUrlAsync(ICurrentRequest currentRequest, string action, string controller,
            object additionalValues)
        {
            if (currentRequest == null)
                throw new ArgumentException("currentRequest is null");


            var input = new FindPathInput();
            input.DomainDatas = currentRequest.DomainDatas;
            input.DomainId = currentRequest.DomainId.ToString(CultureInfo.InvariantCulture);
            input.IsSecure = currentRequest.IsSecure;
            input.Port = currentRequest.Port;
            input.Datas = new Dictionary<string, string>();
            input.Datas.Add("action", action);
            input.Datas.Add("controller", controller);

            var values = UrlHelper.GetValues(additionalValues);
            if (values != null)
            {
                foreach (var value in values)
                {
                    input.Datas.Add(value.Key, value.Value);
                }
            }

            return await _routeManager.FindDomainPathAsync(input);
        }
    }
}