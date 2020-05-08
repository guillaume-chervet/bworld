using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Routing;

namespace Demo.Mvc.Core.Sites.Core
{
    public class BusinessConfig
    {
        public ICollection<DomainSettings> Domains { get; set; }
    }
}