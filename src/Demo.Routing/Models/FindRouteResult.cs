using System.Collections.Generic;

namespace Demo.Routing.Models
{
    public class FindRouteResult
    {
        public bool IsSuccess { get; set; }
        public string HttpCode { get; set; }
        public string RedirectionUrl { get; set; }
        public IDictionary<string, string> Datas { get; set; }
        public string DomainId { get; set; }
        public string SiteId { get; set; }
        public string RouteId { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public Dictionary<string, string> DomainDatas { get; set; }
        public string RewritePath { get; internal set; }
    }
}