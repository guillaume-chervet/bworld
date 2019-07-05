using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Module
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

        protected override void Action()
        {
            throw new NotImplementedException();
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