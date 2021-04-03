namespace Demo.Mvc.Core.Sites.Core.Command.Site
{
    public class AddSiteInput
    {
        public CurrentRequest Site { get; set; }
        public string ModuleId { get; set; }
        public string CategoryId { get; set; }
        public int? Port { get; set; }
        public bool IsSecure { get; set; }
        public string SiteName { get; set; }
        public string SiteId { get; set; }
    }
}