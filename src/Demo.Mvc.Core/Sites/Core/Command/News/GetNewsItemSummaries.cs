using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.News
{
    public class GetNewsItemSummaries<T> where T : GetNewsItemSummary, new()
    {
        public bool HasPrevious { get; set; }
        public string IdPrevious { get; set; }
        public string IdNext { get; set; }

        public bool HasNext { get;

            set;
        }

        public List<T> Items { get;

            set;
        }

    }
}
