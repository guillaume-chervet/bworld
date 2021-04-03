using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Master
{
    public class SaveMasterInput
    {
        public string ModuleId { get; set; }
        public CurrentRequest Site { get; set; }
        public IList<Element> Elements { get; set; }
    }
}