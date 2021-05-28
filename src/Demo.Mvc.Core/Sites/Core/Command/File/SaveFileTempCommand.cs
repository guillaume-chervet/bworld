using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.Command.File.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Core.Command.File
{
    public class SaveFileTempCommand : Command<SaveFileTempInput, CommandResult<SaveFileResult>>
    {
        private readonly IDataFactory _dataFactory;

        public SaveFileTempCommand(IDataFactory dataFactory)
        {
            _dataFactory = dataFactory;
        }

        protected override async Task ActionAsync()
        {
            var streams = new List<Stream>();

            var contentTypeVideo = "video/mp4";
            if (Input.FileData.ContentType == "video/mp4")
            {
                var itemDataModelFile = new ItemDataModel();
                itemDataModelFile.Module = "Video";
                itemDataModelFile.IsTemporary = true;
                itemDataModelFile.PropertyName = Guid.NewGuid().ToString();
                itemDataModelFile.SiteId = Input.SiteId;
                itemDataModelFile.ParentId = Input.SiteId;

                var fileDataModel = new FileDataModel();
                fileDataModel.Module = "VideoData";
                fileDataModel.PropertyName = Guid.NewGuid().ToString();
                fileDataModel.SiteId = Input.SiteId;
                fileDataModel.FileData.ContentType = contentTypeVideo;
                fileDataModel.FileData.FileName = Input.FileData.Filename;
                fileDataModel.FileData.Stream = Input.FileData.Stream;

                itemDataModelFile.Files.Add(fileDataModel);
                _dataFactory.Add(fileDataModel);

                _dataFactory.Add(itemDataModelFile);

                await _dataFactory.SaveChangeAsync();

                Result.Data = new SaveFileResult();
                Result.Data.Id = itemDataModelFile.Id;

                if (!string.IsNullOrEmpty(fileDataModel.FileData.Url))
                {
                    Result.Data.Url = fileDataModel.FileData.Url;
                }
                // Result.Data.ImageUploadedSize = imageUploadedSize;
                //Result.Data.ImageThumbSize = imageThumbSize;
                Result.Data.Id = itemDataModelFile.Id;
                Result.Data.SiteId = Input.SiteId;
                Result.Data.PropertyName = itemDataModelFile.PropertyName;
            }
            else
            {
                try
                {
                    var contentLength = Input.FileData.FileSize;
                    var fileBytes = new byte[contentLength];
                    Input.FileData.Stream.Read(fileBytes, 0, (int)contentLength);

                    var imageConfig = GetImageConfig();

                    var itemDataModelFile = new ItemDataModel();
                    itemDataModelFile.Module = "Image";
                    itemDataModelFile.IsTemporary = true;
                    itemDataModelFile.PropertyName = Guid.NewGuid().ToString();
                    itemDataModelFile.SiteId = Input.SiteId;
                    itemDataModelFile.ParentId = Input.SiteId;

                    _dataFactory.Add(itemDataModelFile);

                    var imageUploadedSize = AddImage(new ImageConfig {MaxWidth = 1600, MaxHeigth = 1600},
                        itemDataModelFile, "ImageUploaded", streams, fileBytes);
                    var imageThumbSize = AddImage(imageConfig, itemDataModelFile, "ImageThumb", streams, fileBytes);

                    await _dataFactory.SaveChangeAsync();

                    Result.Data = new SaveFileResult();
                    Result.Data.Id = itemDataModelFile.Id;
                    Result.Data.ImageUploadedSize = imageUploadedSize;
                    Result.Data.ImageThumbSize = imageThumbSize;
                    Result.Data.Id = itemDataModelFile.Id;
                    Result.Data.SiteId = Input.SiteId;
                    Result.Data.PropertyName = itemDataModelFile.PropertyName;
                }
                finally
                {
                    foreach (var stream in streams)
                    {
                        stream.Dispose();
                    }
                }
            }
        }

        private ImageConfig GetImageConfig()
        {
            ImageConfig imageConfig;

            if (!string.IsNullOrEmpty(Input.ConfigJson))
            {
                imageConfig = JsonConvert.DeserializeObject<ImageConfig>(Input.ConfigJson);
            }
            else
            {
                imageConfig = new ImageConfig
                {
                    MaxHeigth = 600,
                    MaxWidth = 800
                };
            }
            return imageConfig;
        }

        private ImageSize AddImage(ImageConfig imageConfig, ItemDataModel itemDataModelFile, string propertName,
            IList<Stream> streams, byte[] contents)
        {
            // Si c'est une image, créer un aperçu minuscule
            var imageFormat = ImageUtility.GetImageFormat(Input.FileData.ContentType);

            if (imageFormat != null)
            {
                var fileDataModel = new FileDataModel();
                using var memoryStream = new MemoryStream(contents);
                var stream = ImageUtility.ResizeGdi(memoryStream, imageConfig, imageFormat);
                string contentType;
                string filename = null;
                if (!string.IsNullOrEmpty(imageConfig.TypeMime))
                {
                    contentType = imageConfig.TypeMime;
                    var fileNames = Input.FileData.Filename.Split('.');
                    if (fileNames.Length == 2)
                    {
                        filename = string.Concat(fileNames[0], ".",
                            ImageUtility.GetImageExtention(imageConfig.TypeMime));
                    }
                }
                else
                {
                    contentType = Input.FileData.ContentType;
                    filename = Input.FileData.Filename;
                }

                var imageBusinessModel = new ImageBusinessModel();
                imageBusinessModel.Size = new ImageSize {Heigth = stream.Height, Width = stream.With};

                fileDataModel.Module = "ImageData";
                fileDataModel.PropertyName = propertName;
                fileDataModel.SiteId = Input.SiteId;
                fileDataModel.FileData.ContentType = contentType;
                fileDataModel.FileData.FileName = filename;
                fileDataModel.FileData.Stream = stream.Stream;

                itemDataModelFile.Files.Add(fileDataModel);
                _dataFactory.Add(fileDataModel);

                streams.Add(stream.Stream);

                return new ImageSize {Heigth = stream.Height, Width = stream.With};
            }
            return null;
        }

        public static byte[] ReadFully(Stream input)
        {
            using (var ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}