using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.User.Models
{
    public class SaveUserInput
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AuthorUrl { get; set; }
        public string Email { get; set; }

    }
}