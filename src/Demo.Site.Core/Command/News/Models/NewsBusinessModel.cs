using Demo.Business.Command.Free.Models;

namespace Demo.Business.Command.News.Models
{
    public class NewsBusinessModel : FreeBusinessModel
    {
        public string DisplayMode { get; set; } = "article";
        public int NumberItemPerPage { get; set; } = 10;

    }
}