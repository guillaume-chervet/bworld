using System.IO;

namespace Demo.Mvc.Core.Sites.Core.Command.File.Models
{
    public class GetFileResult
    {
        public string RedirectUrl { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public Stream Stream { get; set; }
        public string SiteId { get; set; }
        public string ParentItemId { get; set; }
    }
}