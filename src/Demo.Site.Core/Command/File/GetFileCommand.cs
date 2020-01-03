using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command.File.Models;
using Demo.Business.Command.News;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.User.Identity;

namespace Demo.Business.Command.File
{
    public class GetFileCommand : Command<GetFileInput, CommandResult<GetFileResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetFileCommand(IDataFactory dataFactory, UserService userService)
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
            var itemRepository = _dataFactory.ItemRepository;

            var module = Input.Module;
            if (string.IsNullOrEmpty(module))
            {
                module = "ImageData";
            }
            if (module.ToLower().Contains("video"))
            {
                var videoFile = (await itemRepository.DownloadsAsync(Input.SiteId, Input.Id,false, true)).FirstOrDefault();
                if (videoFile != null && videoFile.FileData != null)
                {
                    await GetNewsItemCommand.CheckAuthorisationAsync(_userService, _dataFactory.ItemRepository, videoFile.SiteId,
                        videoFile.ParentId, Input.UserId);
                        var fileInfo = new GetFileResult();
                    fileInfo.RedirectUrl = videoFile.FileData.Url;
                    Result.Data = fileInfo;
                }
                return;
            }

            var key = Input.Key;
            FileDataModel file;
            {
                if (string.IsNullOrEmpty(key))
                {
                    file = (await itemRepository.DownloadsAsync(Input.SiteId, Input.Id)).FirstOrDefault();
                }
                else
                {
                    file = await itemRepository.DownloadAsync(Input.SiteId, Input.Id, key, module);
                }
                if (file != null)
                {
                    await GetNewsItemCommand.CheckAuthorisationAsync(_userService, _dataFactory.ItemRepository, file.SiteId,
                       file.ParentId, Input.UserId);
                    var fileData = file.FileData;
                    var fileInfo = new GetFileResult();
                    fileInfo.Filename = fileData.FileName;
                    fileInfo.FileSize = fileData.Length;
                    fileInfo.ContentType = fileData.ContentType;
                    fileInfo.Stream = fileData.Stream;
                    Result.Data = fileInfo;
                    return;
                }
            }

            {
                ItemDataModel imageThumb = null;
                if (string.IsNullOrEmpty(Input.PropertyName))
                {
                    // Cela veux dire qu'il y a directement l'id
                    //  imageThumb = await itemRepository.GetItemFromParentAsync(Input.Id, "ImageData", Input.Key);
                    imageThumb = (await itemRepository.GetItemsAsync(Input.SiteId, new ItemFilters
                    {
                        ParentId = Input.Id,
                        PropertyName = Input.Key,
                        Module = "ImageData"
                    })).FirstOrDefault();
                }
                else
                {
                    // l'id est l'id du parent
                    //  var image =   await itemRepository.GetItemFromParentAsync(Input.Id, "Image", Input.PropertyName);
                    var image = (await itemRepository.GetItemsAsync(Input.SiteId, new ItemFilters
                    {
                        ParentId = Input.Id,
                        PropertyName = Input.PropertyName,
                        Module = "Image"
                    })).FirstOrDefault();
                    if(image != null) {
                    // imageThumb = await itemRepository.GetItemFromParentAsync(image.Id, "ImageData", Input.Key);
                    imageThumb = (await itemRepository.GetItemsAsync(Input.SiteId, new ItemFilters
                    {
                        ParentId = image.Id,
                        PropertyName = Input.Key,
                        Module = "ImageData"
                    })).FirstOrDefault();
                    }
                }

                if(imageThumb == null) {
                Result.ValidationResult.AddError("NOT_FOUND");
                 return;
                }
                
                var fileInfoTemp = (OldFileData) imageThumb.Data;

                var fileInfo = new GetFileResult();
                fileInfo.Filename = fileInfoTemp.Filename;
                fileInfo.FileSize = fileInfoTemp.FileSize;
                fileInfo.ContentType = fileInfoTemp.ContentType;
                fileInfo.Stream = new MemoryStream(fileInfoTemp.Contents);
                Result.Data = fileInfo;
            }
        }

        [Obsolete("a supprimer")]
        public class OldFileData
        {
            public string Filename { get; set; }
            public string ContentType { get; set; }
            public int FileSize { get; set; }
            public byte[] Contents { get; set; }
        }
    }
}