using System.Collections.Generic;

namespace Demo.Business.Command.Site.Menu
{
    public class MenuItem
    {
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public IList<MenuItem> Childs { get; set; }
    }
}