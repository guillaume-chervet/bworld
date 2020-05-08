using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Master
{
    public class GetMasterCommand : Command<UserInput<string>, CommandResult<MasterBusinessModel>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetMasterCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, Input.Data);


            var itemDataModel = (await
                    _dataFactory.ItemRepository.GetItemsAsync(Input.Data, new ItemFilters {ParentId = Input.Data, Module = MasterBusinessModule.ModuleName }))
                .FirstOrDefault();

            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

            var moduleFree = (MasterBusinessModel) itemDataModel.Data;

            //TODO Remove
            var metaKeywords = moduleFree.Elements.FirstOrDefault(e => e.Property == "MetaKeyword");
            if (metaKeywords != null)
            {
                moduleFree.Elements.Remove(metaKeywords);
            }
            // TODO Remove
            var metaDescription = moduleFree.Elements.FirstOrDefault(e => e.Property == "MetaDescription");
            if (metaDescription != null)
            {
                moduleFree.Elements.Remove(metaDescription);
            }

            Result.Data = moduleFree;
        }
    }
}