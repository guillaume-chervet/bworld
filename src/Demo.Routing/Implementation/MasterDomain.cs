using System.Collections.Generic;
using Demo.Routing.Interfaces;

namespace Demo.Routing.Implementation
{
    public class MasterDomain
    {
        public string Id { get; set; }
        public string TehcnicalName { get; set; }
        public IList<Domain> Domains { get; set; }
    }
}