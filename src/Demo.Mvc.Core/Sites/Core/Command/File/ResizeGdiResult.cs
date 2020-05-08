using System.IO;

namespace Demo.Mvc.Core.Sites.Core.Command.File
{
    public class ResizeGdiResult
    {
        public Stream Stream { get; set; }
        public int With { get; set; }
        public int Height { get; set; }
    }
}