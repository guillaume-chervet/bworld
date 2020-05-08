using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Data;

namespace Demo.Mvc.Core.Sites.Core.Command.Social
{
    public class GetSocialCommand : Command<GetModuleInput, CommandResult<GetSocialResult>>
    {
        private readonly IDataFactory _dataFactory;

        public GetSocialCommand(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        protected override async Task ActionAsync()
        {
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(Input.SiteId, Input.ModuleId);

            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

            var moduleFree = (SocialBusinessModel) itemDataModel.Data;
            Result.Data = new GetSocialResult() {Data = moduleFree};
        }
    }
}