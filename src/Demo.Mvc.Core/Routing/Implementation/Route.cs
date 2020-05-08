using System.Collections.Generic;

namespace Demo.Mvc.Core.Routing.Implementation
{
    public class Route
    {
        public string Identity { get; set; }
        public string Path { get; set; }
        public string RedirectPath { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public string Namespace { get; set; }
        public string Regex { get; set; }
        public bool CancelRedirect { get; set; }
        public IDictionary<string, string> DefaultValues { get; set; }

        public string RewritePath { get; set; }
    }
}