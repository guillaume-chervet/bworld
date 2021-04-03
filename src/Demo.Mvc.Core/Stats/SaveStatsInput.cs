namespace Demo.Mvc.Core.Stats
{
    public class SaveStatsInput
    {
        public string UserId { get; set; }
        public string IpAdress { get; set; }
        public string UserAgent { get; set; }
        public SaveStatsClientInput Client { get; set; }
    }
}