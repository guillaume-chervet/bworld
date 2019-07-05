using System.Collections.Generic;
using Demo.Routing.Interfaces;

namespace Demo.Routing.Implementation
{
    public class MenuItem
    {
        public int Index { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public IDictionary<string, string> DefaultValues { get; set; }
    }
}