using System.Collections.Generic;
using Demo.User.Identity;

namespace Demo.User
{
    public class MapUser
    {
        public static Demo.User.User Map(ApplicationUser applicationUser)
        {
            if (applicationUser == null)
            {
                return null;
            }

            var user = new Demo.User.User();
            user.Email = applicationUser.Email;
            user.Id = applicationUser.Id;
            user.UserName = applicationUser.FullName;
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.EmailConfirmed = applicationUser.EmailConfirmed;
            user.AuthorUrl = applicationUser.AuthorUrl;
            user.Roles = new List<string>();
            if (applicationUser.Roles != null)
            {
                foreach (var role in applicationUser.Roles)
                {
                    user.Roles.Add(role);
                }
            }
            user.Logins = new List<string>();
            if (applicationUser.Logins != null)
            {
                foreach (var login in applicationUser.Logins)
                {
                    user.Logins.Add(login.LoginProvider);
                }
            }

            return user;
        }
    }
}