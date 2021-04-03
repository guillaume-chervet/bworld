using System.Collections.Generic;

namespace Demo.Mvc.Core.Routing.Implementation
{
    public class MasterDomain
    {
        public string Id { get; set; }
        public string TehcnicalName { get; set; }
        public IList<Domain> Domains { get; set; }
    }
}