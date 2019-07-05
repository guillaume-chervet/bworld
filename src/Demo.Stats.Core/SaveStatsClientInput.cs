using Demo.Data.Stat.Models;

namespace Demo.Business.Command.Stats.Models
{
    public class SaveStatsClientInput
    {
        public string UserId { get; set; }
        public string ClientSessionId { get; set; }
        public string CookieSessionId { get; set; }
        public string Referrer { get; set; }
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string Url { get; set; }
        public UniversType Type { get; set; }
    }
}