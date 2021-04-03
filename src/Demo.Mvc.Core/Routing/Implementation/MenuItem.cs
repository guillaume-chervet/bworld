using System.Collections.Generic;

namespace Demo.Mvc.Core.Routing.Implementation
{
    public class MenuItem
    {
        public int Index { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public IDictionary<string, string> DefaultValues { get; set; }
    }
}