using System.Threading.Tasks;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Data;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.Command;
using Demo.Mvc.Core.Sites.Data;

namespace Demo.Mvc.Core.Message
{
    public class MessageSiteMap : ISiteMap
    {
        private readonly IDataFactory _dataFactory;
        private readonly IRouteManager _routeManager;

        public MessageSiteMap(IDataFactory dataFactory, IRouteManager routeManager)
        {
            _dataFactory = dataFactory;
            _routeManager = routeManager;
        }

        public Task<string> GetSiteNameAsync(string siteId)
        {
            return SiteMap.GetSiteNameAsync(_dataFactory, siteId);
        }

        public async Task<Business.Command.Contact.Message.SiteMap.Site> SiteUrlAsync(string siteId)
        {
            var site = await SiteMap.SiteUrlAsync(_routeManager, _dataFactory, siteId);
            var result = new Business.Command.Contact.Message.SiteMap.Site
            {
                Name = site.Name,
                Url = site.Url
            };
            return result;
        }
    }
}