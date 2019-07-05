
using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Business.Command.User
{
    public class DeleteUserCommand : Command<UserInput<string>, CommandResult>
    {
        private readonly UserService _userService;
        private readonly SiteUserService _siteUserService;

        public DeleteUserCommand(UserService userService, SiteUserService siteUserService)
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
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            var siteUsers = await _siteUserService.FindUsersByUserIdAsync(Input.UserId);

            foreach (var siteUser in siteUsers)
            {
                siteUser.UserId = null;
                await _siteUserService.SaveAsync(siteUser);
            }

            _userService.RemoveAsync(Input.Data);
        }
    }
}