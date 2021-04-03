using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Module
{
    public class DeleteModuleCommand : Command<UserInput<DeleteModuleInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;

        public DeleteModuleCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager)
        {
            _userService = userService;
            _dataFactory = dataFactory;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, Input.Data.Site.SiteId);

            await _dataFactory.DeleteAsync<DataModelBase>(Input.Data.Site.SiteId, Input.Data.ModuleId);

            await _dataFactory.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(Input.Data.Site.SiteId);

            Result.Data = await _moduleManager.GetMasterAsync(Input.Data.Site);
        }
    }
}