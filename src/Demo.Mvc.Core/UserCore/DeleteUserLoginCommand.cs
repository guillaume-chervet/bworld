using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.User.Identity;

namespace Demo.Mvc.Core.UserCore
{
    public class DeleteUserLoginCommand : Command<UserInput<DeleteUserLoginInput>, CommandResult>
    {
        private readonly UserService _userService;

        public DeleteUserLoginCommand(UserService userService)
        {
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            var user = await _userService.FindApplicationUserByIdAsync(Input.UserId);
            var provider = Input.Data.Provider;

            if (user != null && user.Logins != null)
            {
                if (user.Logins.Count() <= 1)
                {
                    Result.ValidationResult.AddError("CANNOT_DELETE_LAST_LOGIN");
                    return;
                }

                var login = user.Logins.FirstOrDefault(l => l.LoginProvider == provider);
                if (login != null)
                {
                    user.RemoveLogin(login.LoginProvider, login.ProviderKey);

                    await _userService.SaveAsync(user);
                    return;
                }
                Result.ValidationResult.AddError("LOGIN_NOT_FOUND");
                return;
            }
            Result.ValidationResult.AddError("USER_NOT_FOUND");
        }
    }
}