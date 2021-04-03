using System;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.User;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Log.Core
{
    public class GetLogCommand : Command<UserInput<GetLogsInput>, CommandResult<dynamic>>
    {
        private readonly ILogService _logService;
        private readonly UserService _userService;

        public GetLogCommand(ILogService logService, UserService userService)
        {
            _logService = logService;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            Result.Data = await _logService.GetLogs(Input.Data);
        }
    }
}