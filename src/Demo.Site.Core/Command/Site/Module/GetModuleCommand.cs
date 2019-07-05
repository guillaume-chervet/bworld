using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Module
{
    public class GetModuleCommand : Command<UserInput<GetModuleInput>, CommandResult<Item>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetModuleCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);
            
            if (string.IsNullOrEmpty(Input.Data.ModuleId))
            {
                throw new ArgumentException("L'id du module est null ou vide");
            }

            var item =
                DataFactoryMongo.MapItemDataModelToItem(
                    await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId));

            Result.Data = item;
        }
    }
}