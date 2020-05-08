using Demo.Mvc.Core.Sites.Core.Command.Free;

namespace Demo.Mvc.Core.Sites.Core.Command.News.Models
{
    public class GetNewsItemBase : GetFreeResult
    {
        public string ModuleId { get; set; }
        public string ParentTitle { get; set; }
        public string ParentModuleId { get; set; }
    }
}