using Demo.Mvc.Core.Sites.Core.Command.Free.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.Social
{
    public class SaveSocialInput : SaveModuleInputBase
    {
        public SocialBusinessModel Data { get; set; }
    }
}