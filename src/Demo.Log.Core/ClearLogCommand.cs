using System;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Log.Core
{
    public class ClearLogCommand : Command<UserInput<string>, CommandResult<dynamic>>
    {
        private readonly ILogService _logService;
        private readonly UserService _userService;

        public ClearLogCommand(ILogService _logService, UserService _userService)
        {
            this._logService = _logService;
            this._userService = _userService;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            await _logService.ClearLogsAsync();
        }
    }
}