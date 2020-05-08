using System.IO;

namespace Demo.Mvc.Core.Sites.Data.Model
{
    public class FileDataInfo
    {
        public Stream Stream { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
        public string Url { get; internal set; }
    }
}