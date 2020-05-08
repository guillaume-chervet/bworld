namespace Demo.Mvc.Core.UserCore.SiteData
{
    public class SaveUserDataInput
    {
        public string Id { get; set; }
        public string CookieSessionId { get; set; }
        public string ModuleId { get; set; }
        public string ElementId { get; set; }
        public string SiteId { get; set; }
        public string Type { get; set; } = "form";
        public long EndTicks { get; set; }
        public long BeginTicks { get; set; }
        public string Json { get; set; }
    }
}
