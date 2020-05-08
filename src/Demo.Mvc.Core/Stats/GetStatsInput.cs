using System;

namespace Demo.Mvc.Core.Stats
{
    public class GetStatsInput
    {
        public DateTime? Date { get; set; }
        public string SiteId { get; set; }
    }
}