using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Module
{
    public class SuperAdminDeleteModuleCommand : Command<UserInput<GetModuleInput>, CommandResult<string>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SuperAdminDeleteModuleCommand(IDataFactory dataFactory, UserService userService,
            CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        protected async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);
            
            var item = await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId);
            await _dataFactory.DeleteAsync<DataModelBase>(item);
            await _dataFactory.SaveChangeAsync();

            await _cacheProvider.UpdateCacheAsync(item.SiteId);
        }
    }
}