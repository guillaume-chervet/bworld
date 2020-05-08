using System.Collections.Generic;

namespace Demo.Mvc.Core.Tags
{
    public class SaveTagsInput
    {
        public string SiteId { get; set; }
        public string ModuleId { get; set; }
        public string Type { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}