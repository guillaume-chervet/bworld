namespace Demo.Mvc.Core.Stats
{
    public class GetStatsResult
    {
        public dynamic Pages { get; set; }
        public int NbNewVisitor { get; set; }
        public int NbVisit { get; set; }
        public dynamic Hours { get; set; }
        public dynamic Referrers { get; set; }
        public dynamic Regions { get; set; }
        public dynamic Devices { get; set; }
        public int NbPageView { get; internal set; }
    }
}