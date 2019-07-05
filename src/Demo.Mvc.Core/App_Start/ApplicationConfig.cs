namespace Demo.Mvc.Core
{
    public class ApplicationConfig
    {
        public bool MinifyHtml { get; set; }
        public string Version { get; set; }
        public string CookieDomain { get; set; }
        public string MainDomainUrl { get; set; }
        public bool IsDebug { get; set; }
        public string Hash { get; set; }
    }
}