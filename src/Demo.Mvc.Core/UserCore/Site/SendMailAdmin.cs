using Demo.User.Identity;

namespace Demo.Mvc.Core.UserCore.Site
{
    public class SendMailAdmin
    {
        public string Mail { get; set; }
        public ApplicationUser UserCreator { get; set; }
        public string SiteId { get; set; }
        public ApplicationUser UserCreated { get; set; }
    }
}
