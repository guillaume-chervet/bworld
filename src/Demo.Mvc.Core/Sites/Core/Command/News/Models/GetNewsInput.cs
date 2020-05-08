using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.Command.News.Models
{
    public class GetNewsInput
    {
        public string SiteId { get; set; }
        public string ModuleId { get; set; }
        public int? FilterIndex { get; set; }
        public IList<ItemState> States { get; set; }
        public IList<string> Tags { get; set; }
    }
}