using System.Collections.Generic;

namespace Demo.Mvc.Core.User
{
    public class User
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string AuthorUrl { get; set; }
        public virtual IList<string> Roles { get; set; }
        public IList<string> Logins { get; set; }
        public bool EmailConfirmed { get; set; }
    }
}