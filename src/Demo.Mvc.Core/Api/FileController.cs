using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Core.Command.File;
using Demo.Mvc.Core.Sites.Core.Command.File.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileResult = Demo.Mvc.Core.Sites.Core.Command.File.Models.FileResult;

namespace Demo.Mvc.Core.Api
{
    public class FileController : ApiControllerBase
    {
        public FileController(BusinessFactory business)
            : base(business)
        {
        }

        [ResponseCache(Duration = 3600000)]
        [HttpGet]
        [Route("api/file/get/{siteid}/{id}/{key}/{filename}")]
        public async Task<IActionResult> Get([FromServices] GetFileCommand _getFileCommand, string siteId, string id, string key, string filename)
        {
            return await GetImageAsync(_getFileCommand, siteId, id, key).ConfigureAwait(false);
        }

        [ResponseCache(Duration = 3600000)]
        [HttpGet]
        [Route("api/file/get/{siteid}/{id}/{propertyName}/{key}/{filename}")]
        public async Task<IActionResult> Get([FromServices]GetFileCommand _getFileCommand,string siteId, string id, string propertyName, string key, string filename)
        {
            return await GetImageAsync(_getFileCommand,siteId, id, key, propertyName).ConfigureAwait(false);
        }

        private async Task<IActionResult> GetImageAsync(GetFileCommand _getFileCommand, string siteId, string id, string key,
            string propertyName = null)
        {
            var fileInfo = new GetFileInput
            {
                SiteId = siteId,
                Id = id,
                Key = key,
                PropertyName = propertyName,
                UserId = StatController.GetUserId(User)
            };
            var result = await
                Business.InvokeAsync<GetFileCommand, GetFileInput, CommandResult<GetFileResult>>(
                    _getFileCommand, fileInfo).ConfigureAwait(false);

            if (result.IsSuccess) return File(result.Data.Stream, result.Data.ContentType);

            return NotFound();
            //return new HttpResponseMessage(HttpStatusCode.NotFound);
        }

        [Authorize]
        [HttpPost]
        [Route("api/file/post")]
        public async Task<FileResult> Post([FromServices] SaveFileTempCommand _saveFileTempCommand)
        {
            return await UploadFileAsync(_saveFileTempCommand, HttpContext).ConfigureAwait(false);
        }

        private async Task<FileResult> UploadFileAsync(SaveFileTempCommand _saveFileTempCommand, HttpContext context)
        {
            var headers = context.Request.Headers;

            FileResult fileResult;
            if (string.IsNullOrEmpty(headers["X-File-Name"]))
                fileResult = await UploadWholeFile(_saveFileTempCommand, context);
            else
                throw new NotImplementedException();

            return fileResult;
        }

        // Upload entire file
        private async Task<FileResult> UploadWholeFile(SaveFileTempCommand _saveFileTempCommand, HttpContext context)
        {
            var fileResult = new FileResult();
            var files = context.Request.Form.Files;
            foreach (var file in files)
            {
                var saveFileTempInput = new SaveFileTempInput();
                saveFileTempInput.SiteId = HttpContext.Request.Form["siteId"];
                saveFileTempInput.ConfigJson = HttpContext.Request.Form["config"];
                var length = file.Length;
                saveFileTempInput.FileData = new FileData
                {
                    Filename = file.FileName,
                    ContentType = file.ContentType,
                    FileSize = length,
                    Stream = file.OpenReadStream()
                };
                var result = await
                    Business.InvokeAsync<SaveFileTempCommand, SaveFileTempInput, CommandResult<SaveFileResult>>(
                        _saveFileTempCommand, saveFileTempInput).ConfigureAwait(false);

                fileResult.Files.Add(FilesStatus(Url.Content("~/"), file.FileName, length, file.ContentType,
                    result.Data));
            }

            return fileResult;
        }

        public FilesStatus FilesStatus(string basePath, string fileName, long fileLength, string type,
            SaveFileResult saveFileResult)
        {
            var filesStatus = new FilesStatus
            {
                Id = saveFileResult.Id,
                SiteId = saveFileResult.SiteId,
                Name = fileName,
                Type = type,
                PropertyName = saveFileResult.PropertyName,
                Size = fileLength
            };
            if (!string.IsNullOrEmpty(saveFileResult.Url))
                filesStatus.Url = basePath + "api/file/get/" + saveFileResult.SiteId + "/" + saveFileResult.Id +
                                  "/ImageUploaded/" + fileName;
            filesStatus.DeleteUrl = basePath + "api/file/delete/" + saveFileResult.SiteId + "/" + saveFileResult.Id +
                                    "/" + fileName;
            filesStatus.DeleteType = "DELETE";
            filesStatus.ThumbnailUrl = basePath + "api/file/get/" + saveFileResult.SiteId + "/" + saveFileResult.Id +
                                       "/ImageThumb/" + fileName;
            filesStatus.IsTemporary = true;

            filesStatus.Detail = new FileDetail
            {
                ImageSize = saveFileResult.ImageUploadedSize,
                Url = filesStatus.Url,
                Size = fileLength
            };
            filesStatus.Detail =
                new FileDetail {ImageSize = saveFileResult.ImageThumbSize, Url = filesStatus.ThumbnailUrl};

            return filesStatus;
        }


        [HttpDelete]
        [Route("api/file/delete/{siteid}/{id}/{filename}")]
        public async Task<HttpResponseMessage> Delete([FromServices]DeleteFileCommand _deleteFileCommand,string siteId, string id, string fileName)
        {
            var result = await
                Business.InvokeAsync<DeleteFileCommand, DeleteFileInput, CommandResult>(_deleteFileCommand,
                    new DeleteFileInput {Id = id, SiteId = siteId}).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("api/file/delete/{siteid}/{id}/{propertyName}/{filename}")]
        public async Task<HttpResponseMessage> Delete([FromServices]DeleteFileCommand _deleteFileCommand, string siteId, string id, string propertyName, string fileName)
        {
            var result = await
                Business.InvokeAsync<DeleteFileCommand, DeleteFileInput, CommandResult>(_deleteFileCommand,
                    new DeleteFileInput
                    {
                        Id = id,
                        PropertyName = propertyName,
                        SiteId = siteId
                    }).ConfigureAwait(false);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}