using System.Collections.Generic;
using Demo.Business.Command.Site;

namespace Demo.Business.Command.User
{
    public class GetUserResult
    {
        public IList<GetSitesResult> GetSites { get; set; }
        public IList<string> Providers { get; set; }
    }
}