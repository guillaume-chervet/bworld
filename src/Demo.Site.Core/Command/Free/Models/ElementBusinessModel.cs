using System.Collections.Generic;

namespace Demo.Business.Command.Free.Models
{
    public abstract class ElementBusinessModel
    {
        public IList<Element> Elements { get; set; }
        public bool IsDisplayAuthor { get; set; }
        public bool IsDisplaySocial { get; set; }
        public string Icon { get; set; }
    }
}