using System.Collections.Generic;
using Demo.Routing.Interfaces;

namespace Demo.Routing.Implementation
{
    public class Site
    {
        public string Id { get; set; }
        public string DefaultCultureId { get; set; }
        public IDictionary<string, string> DefaultValues { get; set; }
    }
}