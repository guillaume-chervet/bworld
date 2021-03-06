﻿using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.Command.File.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command.File
{
    public class DeleteFileCommand : Command<DeleteFileInput, CommandResult>
    {
        private readonly IDataFactory _dataFactory;

        public DeleteFileCommand(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
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