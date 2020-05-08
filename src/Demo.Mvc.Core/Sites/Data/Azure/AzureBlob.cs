using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Data.Azure
{
    public class AzureBlob : IDataBlob
    {
        private readonly StorageConfig _azureConfig;
        private readonly IMongoDatabase _database;
        private readonly IMongoCollection<FileMetadata> _collection;

        public AzureBlob(IOptions<StorageConfig> azureConfig, IDatabase db)
        {
            _database = db.GetDatabase();
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _collection = _database.GetCollection<FileMetadata>("site.file", collectionSettings);
            _azureConfig = azureConfig.Value;
        }

        private async Task<long> Upload(FileDataModel fileDataModel)
        {
            var siteId = fileDataModel.SiteId.ToLower();
            var fileName = GetFileName(fileDataModel);

            var fileDataInfo = fileDataModel.FileData;
            var stream = fileDataInfo.Stream;
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(_azureConfig.StorageConnectionString);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve a reference to a container.
            var container = blobClient.GetContainerReference(siteId);
            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess =
                    BlobContainerPublicAccessType.Off
                    });
            // Retrieve reference to a blob named "myblob".
            var blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = fileDataInfo.ContentType;
            // Create or overwrite the "myblob" blob with contents from a local file.
            await blockBlob.UploadFromStreamAsync(stream);
            
            if (fileDataInfo.Length <= 0)
            {
                // TODO trouver plus propre
                await blockBlob.FetchAttributesAsync();
                fileDataInfo.Length = blockBlob.Properties.Length;
                //_logger.Warn(fileDataModel.SiteId + " " + fileDataInfo.FileName + " Content size not found");
            }

            // TODO trouver un moyen injecter l'info de faire cela
            /*if (!String.IsNullOrEmpty(fileDataInfo.ContentType) && fileDataInfo.ContentType.ToLower().Contains("video"))
            {
                fileDataInfo.Url = GetBlobSasUri(blockBlob);
            }*/

            return fileDataInfo.Length;//;
        }

        private static string GetFileName(FileDataModel fileDataModel)
        {
            var fileName = fileDataModel.FileData.FileName.ToLower();

            if (!string.IsNullOrEmpty(fileDataModel.PropertyName))
            {
                fileName = fileDataModel.ParentId + "-" + fileDataModel.PropertyName + "-" + fileName;
            }
            return fileName;
        }

        public async Task DeleteAsync(string id)
        {
            var builder = Builders<FileMetadata>.Filter;
            var filter = builder.Eq(x => x.Guid, new Guid(id));
            var result = (await _collection.FindAsync(filter)).FirstOrDefault();

            var file = MapFileDataModel(result);
            var fileName = GetFileName(file);
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                _azureConfig.StorageConnectionString);

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference(file.SiteId);

            // Retrieve reference to a blob named "myblob.txt".
            var blockBlob = container.GetBlockBlobReference(fileName);

            // Delete the blob.
            await blockBlob.DeleteAsync();
        }       

        private static FileDataModel MapFileDataModel(FileMetadata result)
        {
            var file = new FileDataModel()
            {
                CreateDate = result.CreateDate,
                UpdateDate = result.UpdateDate,
                Id = result.Id,
                PropertyName = result.PropertyName,
                Index = result.Index,
                SiteId = result.SiteId,
                Module = result.Module,
                ParentId = result.ParentId,
                IsTemporary = result.IsTemporary,
             //   Data = null, //MemoryDatabase.GetItemData(item),
                IsLoadedFromDatabase = true,
                FileData = new FileDataInfo()
                {
                    ContentType = result.ContentType,
                    FileName = result.Filename,
                    Length = result.SizeBytes.GetValueOrDefault()
                }
            };
            return file;
        }

        public async Task UploadAsync(FileDataModel fileDataModel, bool isUploadOnly=false)
        {
            var sizeBytes = await Upload(fileDataModel);
            var fileData = fileDataModel.FileData;
            if (!isUploadOnly)
            {
                var fileMetadata = new FileMetadata()
                {
                    ContentType = fileData.ContentType,
                    Filename = fileData.FileName,
                    PropertyName = fileDataModel.PropertyName,
                    Index = fileDataModel.Index,
                    IsTemporary = fileDataModel.IsTemporary,
                    ParentId = fileDataModel.ParentId,
                    SiteId = fileDataModel.SiteId,
                    SizeBytes = sizeBytes,
                    TypeFile = "0",
                    CreateDate = DateTime.Now,
                    Id = fileDataModel.Id,
                    Module = fileDataModel.Module,
                };

                if (fileDataModel.Data != null)
                {
                    fileMetadata.Json = JsonConvert.SerializeObject(fileDataModel.Data);
                    fileMetadata.Type = fileDataModel.Data.GetType().FullName;
                }

                await _collection.InsertOneAsync(fileMetadata);
            }
        }

        static string GetBlobSasUri(CloudBlockBlob blob)
        {
            //Set the expiry time and permissions for the blob.
            //In this case the start time is specified as a few minutes in the past, to mitigate clock skew.
            //The shared access signature will be valid immediately.
            var sasConstraints = new SharedAccessBlobPolicy();
            sasConstraints.SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5);
            sasConstraints.SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(2);
            sasConstraints.Permissions = SharedAccessBlobPermissions.Read;

            //Generate the shared access signature on the blob, setting the constraints directly on the signature.
            string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

            //Return the URI string for the container, including the SAS token.
            return blob.Uri + sasBlobToken;
        }

        private async Task<IList<FileDataModel>> GetFileDataModelsAsync(string siteId, bool isGetStream, bool isGetUrl, List<FileMetadata> results)
        {
            var files = new List<FileDataModel>();
            foreach (var fileMetadata in results)
            {
                var file = MapFileDataModel(fileMetadata);
                files.Add(file);
                var fileName = GetFileName(file);
                if (isGetStream)
                {
                    var blockBlob = CloudBlockBlob(siteId, fileName);
                    // Save blob contents to a file.
                    var stream = new MemoryStream();
                     await blockBlob.DownloadToStreamAsync(stream);
                    stream.Position = 0;
                    file.FileData.Stream = stream;
                    if (file.FileData.Length <= 0)
                    {
                        await blockBlob.FetchAttributesAsync();
                        file.FileData.Length = blockBlob.Properties.Length;
                    }
                }
                if (isGetUrl)
                {
                    var blockBlob = CloudBlockBlob(siteId, fileName);
                    file.FileData.Url = GetBlobSasUri(blockBlob);
                }
            }
            return files;
        }

        private CloudBlockBlob CloudBlockBlob(string siteId, string fileName)
        {
            var container = CloudBlobContainer(siteId);
            // Retrieve reference to a blob named "photo1.jpg".
            var blockBlob = container.GetBlockBlobReference(fileName);
            return blockBlob;
        }

        private CloudBlobContainer CloudBlobContainer(string siteId)
        {
// Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(
                _azureConfig.StorageConnectionString);
            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();
            // Retrieve reference to a previously created container.
            var container = blobClient.GetContainerReference(siteId);
            return container;
        }


        public async Task<IList<FileDataModel>> DownloadsAsync(string siteId = null, string parentId = null, bool isGetStream = true, bool isGetUrl = false)
        {
            var builder = Builders<FileMetadata>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId);
            if (parentId != null)
            {
                filter = filter & builder.Eq(x => x.ParentId, parentId);
            }
            var results = await _collection.Find(filter).ToListAsync();

            return await GetFileDataModelsAsync(siteId, isGetStream, isGetUrl, results);
        }

        public async Task<FileDataModel> DownloadAsync(string siteId, string parentId, string propertyName, string module, bool isGetStream = true, bool isGetUrl = false)
        {
            var builder = Builders<FileMetadata>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Eq(x => x.PropertyName, propertyName) & builder.Eq(x => x.Module, module) & builder.Eq(x => x.ParentId, parentId);
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var results = new List<FileMetadata>();
            results.Add(result);
            return (await GetFileDataModelsAsync(siteId, isGetStream, isGetUrl, results)).FirstOrDefault();
        }

        public async Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true, bool isGetUrl = false)
        {
            var builder = Builders<FileMetadata>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Eq(x => x.Guid, new Guid(id));
            var result = await _collection.Find(filter).FirstOrDefaultAsync();

            if (result == null)
            {
                return null;
            }

            var results = new List<FileMetadata>();
            results.Add(result);
            return (await GetFileDataModelsAsync(siteId, isGetStream, isGetUrl, results)).FirstOrDefault();
        }

      
    }
}
