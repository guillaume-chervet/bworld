using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Business.Command.Administration.User
{
    public class LoadUserCommand : Command<UserInput<LoadUserInput>, CommandResult<dynamic>>
    {
        private readonly UserService _userService;
        private readonly SiteUserService _siteUserService;

        public LoadUserCommand(UserService userService, SiteUserService siteUserService)
        {
            _userService = userService;
            _siteUserService = siteUserService;
        }

        protected override async Task ActionAsync()
        {
            var siteUserId = Input.Data.SiteUserId;
            var siteId = Input.Data.SiteId;
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var siteUserDb = await _siteUserService.FindAsync(siteUserId);
            ApplicationUser userDb = null;

            var userId = siteUserDb.UserId;
            if (!string.IsNullOrEmpty(siteUserDb.UserId))
            {
                userDb = await _userService.FindApplicationUserByIdAsync(userId);
            }

            Result.Data = ListUserCommand.MapUserResult(userDb, siteId, siteUserDb);

        }
    }
}
