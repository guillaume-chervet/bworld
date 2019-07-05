using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;

namespace Demo.Business.Command.Site
{
    public class LoadAddSiteCommand : Command<LoadAddSiteInput, CommandResult<dynamic>>
    {
        private readonly IDataFactory _dataFactory;

        public LoadAddSiteCommand(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(Input.SiteId, Input.ModuleId);

            Result.Data = itemDataModel.Data;
        }
    }
}