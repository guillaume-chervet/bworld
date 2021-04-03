using Demo.Mvc.Core.Sites.Core.Command.Free.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.News.Models
{
    public class NewsBusinessModel : FreeBusinessModel
    {
        public string DisplayMode { get; set; } = "article";
        public int NumberItemPerPage { get; set; } = 10;

    }
}