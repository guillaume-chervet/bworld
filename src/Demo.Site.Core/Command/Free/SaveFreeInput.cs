using System.Collections.Generic;
using Demo.Data.Model;

namespace Demo.Business.Command.Free.Models
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