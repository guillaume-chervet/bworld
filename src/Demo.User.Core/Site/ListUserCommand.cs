using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Site;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Business.Command.Administration.User
{
    public class ListUserCommand : Command<UserInput<string>, CommandResult<ListUserResult>>
    {
        private readonly UserService _userService;
        private readonly SiteUserService _siteUserService;

        public ListUserCommand(UserService userService, SiteUserService siteUserService)
        {
            _userService = userService;
            _siteUserService = siteUserService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data;
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var usersDb = await _userService.UserByRoleAsync(siteId);
            var siteUsersDb = await _siteUserService.FindBySiteId(siteId);

            var users = new List<UserResult>();
            foreach (var siteUserDbModel in siteUsersDb)
            {
                var us = usersDb.FirstOrDefault(u => u.Id == siteUserDbModel.UserId);
                var user = MapUserResult(us, siteId, siteUserDbModel);
                users.Add(user);
            }

            Result.Data = new ListUserResult()
            {
                Users = users
            };

        }

        public static UserResult MapUserResult(ApplicationUser us, string siteId, SiteUserDbModel siteUserDbModel)
        {
            var roles = GetRoles(us, siteId, siteUserDbModel);

            var user = new UserResult()
            {
                UserId = siteUserDbModel.UserId,
                SiteUserId = siteUserDbModel.Id,
                MailConfirmed = us?.EmailConfirmed ?? false,
                IsEmailNotif = siteUserDbModel.IsEmailNotif,
                Mail = siteUserDbModel.Mail,
                FirstName = siteUserDbModel.FirstName,
                LastName = siteUserDbModel.LastName,
                FullName = siteUserDbModel.FullName,
                Civility = siteUserDbModel.Civility,
                Birthdate = siteUserDbModel.Birthdate,
                Comments = siteUserDbModel.Comments,
                Tags = siteUserDbModel.Tags,
                Roles = roles,
            };
            return user;
        }

        public static IList<SiteUserRole> GetRoles(ApplicationUser applicationUser, string siteId, SiteUserDbModel siteUserDbModel)
        {
            IList<SiteUserRole> roles = new List<SiteUserRole>();
            if (applicationUser != null)
            {
                var userRoles = (SiteUserRole[]) Enum.GetValues(typeof(SiteUserRole));
                foreach (var siteUserRole in userRoles)
                {
                    var role = UserSecurity.MapRole(siteId, siteUserRole);
                    if (applicationUser.Roles.Count(c=>c==role) > 0)
                    {
                        roles.Add(siteUserRole);
                    }
                }

            }
            if (siteUserDbModel != null && siteUserDbModel.FlaggedRoles != null)
            {
                foreach (var siteUserRole in siteUserDbModel.FlaggedRoles)
                {
                    if (!roles.Contains(siteUserRole))
                    {
                        roles.Add(siteUserRole);
                    }
                }
            }

            return roles;
        }
    }
}
