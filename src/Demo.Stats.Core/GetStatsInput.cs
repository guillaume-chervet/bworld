using System;

namespace Demo.Business.Command.Stats.Models
{
    public class GetStatsInput
    {
        public DateTime? Date { get; set; }
        public string SiteId { get; set; }
    }
}