using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.User
{
    public class GetUsersCommand : Command<UserInput<string>, CommandResult<IList<Demo.User.User>>>
    {
        private readonly UserService _userService;

        public GetUsersCommand(UserService userService)
        {
            _userService = userService;
        }
        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            IList<Demo.User.User> users=  new List<Demo.User.User>();
            var applicationUser = await _userService.GetAllAsync();
            foreach (var user in applicationUser)
            {
                users.Add(MapUser.Map(user));
            }

            Result.Data = users;
        }
    }
}