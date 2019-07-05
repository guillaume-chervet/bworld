using System;
using System.Threading.Tasks;
using Demo.Business.Command.File.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Repository;

namespace Demo.Business.Command.File
{
    public class DeleteFileCommand : Command<DeleteFileInput, CommandResult>
    {
        private readonly IDataFactory _dataFactory;

        public DeleteFileCommand(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            if (string.IsNullOrEmpty(Input.PropertyName))
            {
                var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(Input.SiteId, Input.Id);
                await _dataFactory.DeleteAsync(itemDataModel);
            }
            else
            {
                var itemDataModels =
                    await
                        _dataFactory.ItemRepository.GetItemsAsync(Input.SiteId,
                            new ItemFilters {ParentId = Input.Id, Module = "Image", PropertyName = Input.PropertyName});
                foreach (var dataModel in itemDataModels)
                {
                    await _dataFactory.DeleteAsync(dataModel);
                }
            }
            
            await _dataFactory.SaveChangeAsync();
        }
    }
}