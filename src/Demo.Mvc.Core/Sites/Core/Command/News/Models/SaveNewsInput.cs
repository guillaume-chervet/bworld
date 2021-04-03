using Demo.Mvc.Core.Sites.Core.Command.Free;

namespace Demo.Mvc.Core.Sites.Core.Command.News.Models
{
    public class SaveNewsInput : SaveFreeInput
    {
        public string DisplayMode { get; set; }
        public int NumberItemPerPage { get; set; } = 10;
    }
}