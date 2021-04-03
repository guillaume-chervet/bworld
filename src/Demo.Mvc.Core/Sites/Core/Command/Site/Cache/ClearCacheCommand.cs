using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Cache
{
    public class ClearCacheCommand : Command<UserInput<string>, CommandResult>
    {
        private readonly UserService _userService;
        private readonly CacheProvider _cacheProvider;

        public ClearCacheCommand(UserService userService, CacheProvider cacheProvider)
        {
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        protected override async Task ActionAsync()
        {
          //  await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);

            await _cacheProvider.InitializeCacheAsync();
        }
    }
}
