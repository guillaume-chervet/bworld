using Demo.User.Identity.Models;

namespace Demo.Business.Command.Free.Models
{
    public class UserInfoResult
    {
        public UserInfo LastUpdateUserInfo { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}