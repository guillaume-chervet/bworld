using System.Collections.Generic;

namespace Demo.Mvc.Core.Routing.Implementation
{
    public class Site
    {
        public string Id { get; set; }
        public string DefaultCultureId { get; set; }
        public IDictionary<string, string> DefaultValues { get; set; }
    }
}