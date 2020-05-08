using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free;

namespace Demo.Mvc.Core.Sites.Core.Command.News.Models
{
    public class GetNewsResult : GetFreeResult
    {
        public string DisplayMode { get; set; }
        public int NumberItemPerPage { get; set; }
        public IList<GetNewsItemSummary> GetNewsItem { get; set; }
        public bool HasPrevious { get; set; }
        public string IdPrevious { get; set; }
        public bool HasNext { get; set; }
        public string IdNext { get; set; }
        public string ModuleId { get; set; }
    }
}