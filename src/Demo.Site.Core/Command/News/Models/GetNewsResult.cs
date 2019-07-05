using System;
using System.Collections.Generic;
using Demo.Business.Command.Free.Models;
using Demo.User.Identity.Models;

namespace Demo.Business.Command.News.Models
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