using System.Threading.Tasks;

namespace Demo.Business.Command.Contact.Message.SiteMap
{
    public interface ISiteMap
    {
        Task<string> GetSiteNameAsync(string siteId);
        Task<Site> SiteUrlAsync(string siteId);
    }
}