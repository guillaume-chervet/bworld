using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.News.Models;

namespace Demo.Business.Command.News
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
