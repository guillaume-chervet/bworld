using System.Collections.Generic;
using Demo.Business.Routing;

namespace Demo.Business.Tests
{
    public class CurrentCurrentRequest : ICurrentRequest
    {
        public string CultureId
        {
            get { return "11"; }
        }

        public bool IsSecure
        {
            get { return false; }
        }

        public bool? IsForceSecure
        {
            get { return false; }
        }

        public string SiteId
        {
            get { return "1"; }
        }

        public string DomainId
        {
            get { return "1"; }
        }

        public string Port
        {
            get { return string.Empty; }
        }

        public IDictionary<string, string> DomainDatas
        {
            get
            {
                var domainDatas = new Dictionary<string, string>();
                domainDatas.Add("site", "MyWorld");
                domainDatas.Add("domain", "site.demo.fr");

                return domainDatas;
            }
        }
    }
}