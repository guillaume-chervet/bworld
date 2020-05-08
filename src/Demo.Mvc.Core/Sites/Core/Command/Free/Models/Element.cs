using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Command.Free.Models
{
    public class Element
    {
        public string Type { get; set; }
        public string Property { get; set; }
        public IList<Element> Childs { get; set; }
        public string Data { get; set; }
    }
}