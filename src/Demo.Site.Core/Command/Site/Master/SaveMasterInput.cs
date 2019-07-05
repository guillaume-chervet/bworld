using System.Collections.Generic;
using Demo.Business.Command.Free.Models;

namespace Demo.Business.Command.Site.Master
{
    public class SaveMasterInput
    {
        public string ModuleId { get; set; }
        public CurrentRequest Site { get; set; }
        public IList<Element> Elements { get; set; }
    }
}