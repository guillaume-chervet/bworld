using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Site;

namespace Demo.Mvc.Core.Sites.Core.Command.User
{
    public class GetUserResult
    {
        public IList<GetSitesResult> GetSites { get; set; }
        public IList<string> Providers { get; set; }
    }
}