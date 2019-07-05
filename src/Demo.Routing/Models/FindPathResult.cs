using Demo.Routing.Implementation;
using Demo.Routing.Interfaces;

namespace Demo.Routing.Models
{
    public class FindPathResult
    {
        public bool IsSuccess { get; set; }
        public string Path { get; set; }
        public bool IsSecure { get; set; }
        public string PreUrl { get; set; }
        public string RequestDomain { get; set; }
        public string RoutePath { get; set; }
        public Route Route { get; set; }
        public string RoutePathWithoutHomePage { get; set; }
        public string FullUrl { get; set; }
        public string BaseUrl { get; set; }
    }
}