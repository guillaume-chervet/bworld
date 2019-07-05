using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Data.Tests
{
    [TestClass]
    public class BlobUnitTest
    {
       

        [TestMethod]
        public void UploadBlob()
        {
         //   AzureBlob azureBlob = new AzureBlob(new DataConfig(), new DataConfig());

            using (var stream = File.OpenRead(@"C:\test.jpg"))
            {
               // azureBlob.UploadAsync("siteId", "fileName", stream);
            }


        }

      /*  [TestMethod]
        public async Task TransfertBlob()
        {
            var azureBlob = new AzureBlob(new DataConfig(), new DataConfig());
            var mongoBlob = new MongoBlob(new DataConfig());

            var dataConfig = new DataConfig();
            var client = new MongoClient(dataConfig.MongoConnectionString);
            var database = client.GetDatabase(dataConfig.MongoDatabaseName);

            var siteRepository = new ItemRepositoryMongo(database, new MemorySession<ItemDataModelBase>(), new MongoBlob(new DataConfig()));
            var siteDataModels = await GetSitesCommand.GetSiteDataModelsAsync(siteRepository);

            foreach (var siteDataModel in siteDataModels)
            {
               
            //var list = mongoBlob.Downloads("227aefdb-a2b9-4c27-98d9-2f0db43f99ca");
            var list = await mongoBlob.DownloadsAsync(siteDataModel.Id);

                foreach (var fileDataModel in list)
                {
                   Logger.Default.Info(fileDataModel.SiteId + "-"+ fileDataModel.ParentId+"-"+fileDataModel.PropertyName+"-"+fileDataModel.FileData.FileName);
                   await azureBlob.UploadAsync(fileDataModel);
                    fileDataModel.FileData.Stream.Close();
                    fileDataModel.FileData.Stream.Dispose();
                }
            }

            /* using (var stream = File.OpenRead(@"C:\test.jpg"))
             {
                 azureBlob.Upload("siteId", "fileName", stream);
             }


        }*/


       /* [TestMethod]
        public async Task TransfertBlobToFile()
        {
            var azureBlob = new AzureBlob(new DataConfig(), new DataConfig());

            var dataConfig = new DataConfig();
            var client = new MongoClient(dataConfig.MongoConnectionString);
            var database = client.GetDatabase(dataConfig.MongoDatabaseName);

            var siteRepository = new ItemRepositoryMongo(database, new MemorySession<ItemDataModelBase>(), new MongoBlob(new DataConfig()));
            var siteDataModels = await GetSitesCommand.GetSiteDataModelsAsync(siteRepository);

            foreach (var siteDataModel in siteDataModels)
            {

                //var list = mongoBlob.Downloads("227aefdb-a2b9-4c27-98d9-2f0db43f99ca");
                var list = await azureBlob.DownloadsAsync(siteDataModel.Id);

                foreach (var fileDataModel in list)
                {
                    var fileName = fileDataModel.ParentId + "-" + fileDataModel.PropertyName + "-" + fileDataModel.FileData.FileName;
                     Logger.Default.Info(fileDataModel.SiteId + "-" + fileName);
                    if (!Directory.Exists("C:\\bworld"))
                    {
                        Directory.CreateDirectory("C:\\bworld");
                    }
                    var fullPath = @"C:\\bworld\\" + fileDataModel.SiteId;
                    if (!Directory.Exists(fullPath))
                    {
                        Directory.CreateDirectory(fullPath);
                    }


                    using (var fileStream = System.IO.File.OpenWrite(Path.Combine(fullPath, fileName)))
                    {
                        var ms = fileDataModel.FileData.Stream;
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        fileStream.Write(bytes, 0, bytes.Length);
                        ms.Close();
                        ms.Dispose();
                    }

                   /* using (FileStream file = new FileStream(Path.Combine(fullPath, fileName), FileMode.Create, System.IO.FileAccess.Write))
                    {
                        var ms = fileDataModel.FileData.Stream;
                        byte[] bytes = new byte[ms.Length];
                        ms.Read(bytes, 0, (int)ms.Length);
                        file.Write(bytes, 0, bytes.Length);
                        ms.Close();
                        ms.Dispose();
                    }
                    //var stream = File.Create(Path.Combine(fullPath, fileName));

                    //await stream.CopyToAsync(fileDataModel.FileData.Stream);

                    //fileDataModel.FileData.Stream.Dispose();
                   // stream.Dispose();
                }
            }

            /* using (var stream = File.OpenRead(@"C:\test.jpg"))
             {
                 azureBlob.Upload("siteId", "fileName", stream);
             }


        }*/

    }
}