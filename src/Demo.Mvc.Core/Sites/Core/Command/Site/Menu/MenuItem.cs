using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Menu
{
    public class MenuItem
    {
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public IList<MenuItem> Childs { get; set; }
    }
}