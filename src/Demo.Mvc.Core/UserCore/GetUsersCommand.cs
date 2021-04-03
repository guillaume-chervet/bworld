using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.UserCore
{
    public class GetUsersCommand : Command<UserInput<string>, CommandResult<IList<User.User>>>
    {
        private readonly UserService _userService;

        public GetUsersCommand(UserService userService)
        {
            _userService = userService;
        }
        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            IList<User.User> users=  new List<User.User>();
            var applicationUser = await _userService.GetAllAsync();
            foreach (var user in applicationUser)
            {
                users.Add(MapUser.Map(user));
            }

            Result.Data = users;
        }
    }
}