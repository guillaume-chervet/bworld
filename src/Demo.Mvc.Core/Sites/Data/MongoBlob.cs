using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Data
{
    public class MongoBlob : IDataBlob
    {
        private readonly IMongoDatabase _database;

        public MongoBlob(IDatabase db)
        {
            _database = db.GetDatabase();
        }

        public async Task<IList<FileDataModel>> DownloadsAsync(string siteId = null, string parentId = null, bool isGetStream = true, bool isGetUrl = false)
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Empty;
            if (!string.IsNullOrEmpty(siteId))
            {
                    filter = builder.Eq("metadata.id", siteId) ;
                if (!string.IsNullOrEmpty(parentId))
                {
                    filter = filter & builder.Eq("metadata.parentId", parentId);
                }
            }
            var gridFs = new GridFSBucket(_database);
            var files = await gridFs.Find(filter).ToListAsync();
            var results = new List<FileDataModel>();
            if (files == null)
            {
                return results;
            }
            foreach (var mongoGridFsFileInfo in files)
            {
                var fileDataModel = MapFileDataModel(mongoGridFsFileInfo);
                if (isGetStream)
                {
                    fileDataModel.FileData.Stream = await gridFs.OpenDownloadStreamAsync(mongoGridFsFileInfo.Id);
                }
                results.Add(fileDataModel);
            }

            return results;
        }

        /*public IList<FileDataModel> GetFiles(string siteId, string parentId)
        {
            return Downloads(siteId, parentId, false);
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq("metadata.id", siteId) & builder.Eq("metadata.parentId", parentId);

            var gridFs = new GridFSBucket(_database);
            var files = gridFs.Find(filter).ToList();

            var results = new List<FileDataModel>();
            if (files == null)
            {
                return results;
            }

            foreach (var mongoGridFsFileInfo in files)
            {
                var fileDataModel = MapFileDataModel(mongoGridFsFileInfo);
                results.Add(fileDataModel);
            }

            return results;
    }*/

        public async Task<FileDataModel> DownloadAsync(string siteId, string parentId, string propertyName, string module, bool isGetStream = true, bool isGetUrl = false)
        {
            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq("metadata.id", siteId) & builder.Eq("metadata.parentId", parentId) & builder.Eq("metadata.propertyName", propertyName) & builder.Eq("metadata.module", module);

            var gridFs = new GridFSBucket(_database);
            var file = await gridFs.Find(filter).FirstOrDefaultAsync();

            if (file == null)
            {
                return null;
            }

            var fileDataModel = MapFileDataModel(file);
            fileDataModel.FileData.Stream = await gridFs.OpenDownloadStreamAsync(file.Id);
            return fileDataModel;
        }

        public async Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true, bool isGetUrl = false)
        {
            var oid = new ObjectId(id);

            var builder = Builders<GridFSFileInfo>.Filter;
            var filter = builder.Eq("metadata.id", siteId) & builder.Eq("_id", oid);

            var gridFs = new GridFSBucket(_database);
            var file = await gridFs.Find(filter).FirstOrDefaultAsync();

            if (file == null)
            {
                return null;
            }

            var fileDataModel = MapFileDataModel(file);
            fileDataModel.FileData.Stream = await gridFs.OpenDownloadStreamAsync(file.Id);
            return fileDataModel;
        }

        private static FileDataModel MapFileDataModel(GridFSFileInfo file)
        {
            var document = file.Metadata;
            var fileDataModel = new FileDataModel();
            fileDataModel.Id = file.Id.ToString();
            fileDataModel.SiteId = document["id"].ToString();
            fileDataModel.Index = document["index"].ToInt32();
            fileDataModel.IsTemporary = document["isTemporary"].ToBoolean();
            fileDataModel.Module = document["module"].ToString();
            fileDataModel.ParentId = document["parentId"].ToString();
            fileDataModel.PropertyName = document["propertyName"].ToString();
            fileDataModel.CreateDate = document["createDate"].ToUniversalTime();
            if (document["updateDate"] != BsonNull.Value)
            {
                fileDataModel.UpdateDate = document["updateDate"].ToUniversalTime();
            }

            // TODO
            /* if ( document["json"]  != null)
            {
                fileDataModel.Data = JsonConvert.SerializeObject();
            }*/

            fileDataModel.FileData.FileName = file.Filename;
            fileDataModel.FileData.ContentType = file.ContentType;
            fileDataModel.FileData.Length = file.Length;
            fileDataModel.IsLoadedFromDatabase = true;
            return fileDataModel;
        }


        
        private void InitFileDocument(BsonDocument document, FileDataModel fileDataModel)
        {
            document["id"] = fileDataModel.SiteId;
            document["type"] = 0;
            document["index"] = fileDataModel.Index;
            document["isTemporary"] = fileDataModel.IsTemporary;
            document["module"] = fileDataModel.Module;
            document["parentId"] = fileDataModel.ParentId;
            document["propertyName"] = fileDataModel.PropertyName;
            document["createDate"] = fileDataModel.CreateDate;
            document["updateDate"] = fileDataModel.UpdateDate;
            if (fileDataModel.Data != null)
            {
                document["json"] = JsonConvert.SerializeObject(fileDataModel.Data);
            }
        }


        public async Task UploadAsync(FileDataModel fileDataModel, bool isUpoadOnly=false)
        {
            var document = new BsonDocument();
            InitFileDocument(document, fileDataModel);

            var gridFs = new GridFSBucket(_database);
            var gridFsInfo = await gridFs.UploadFromStreamAsync(fileDataModel.FileData.FileName, fileDataModel.FileData.Stream, 
                new GridFSUploadOptions
                {
                    Metadata = document,
                    ContentType = fileDataModel.FileData.ContentType,
                });

            var fileId = gridFsInfo.ToString();

            fileDataModel.Id = fileId;
        }

        public async Task DeleteAsync(string id)
        {
            var gridFs = new GridFSBucket(_database);
            await gridFs.DeleteAsync(new ObjectId(id));
        }

    }
}
