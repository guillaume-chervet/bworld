using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.Command.Free
{
    public class SaveFreeInput : SaveModuleInputBase
    {
        public IList<Element> Elements { get; set; }

        public ItemState State { get; set; }
        public string Icon { get; set; }

        public IList<string> Tags { get; set; }

        public bool IsDisplayAuthor { get; set; }
        public bool IsDisplaySocial { get; set; }
    }
}