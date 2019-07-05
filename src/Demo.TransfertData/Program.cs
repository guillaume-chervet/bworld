using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.Site;
using Demo.Data;
using Demo.Data.Azure;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.Log;
using MongoDB.Driver;

namespace Demo.TransfertData
{
    class Program
    {
        static void Main(string[] args)
        {
            /*var task = DoActionAsync();
            task.Wait();*/
            Console.WriteLine("Fin du batch");
            Console.ReadLine();
        }
/*
        private static async Task DoActionAsync()
        {
            try
            {
                var azureBlobSource = new AzureBlob(new StorageConfig(), new StorageConfig());
                var azureBlobDestination = new AzureBlob(new DestinationDataConfig(), new DestinationDataConfig());

                var dataConfig = new StorageConfig();
                var client = new MongoClient(dataConfig.MongoConnectionString);
                var database = client.GetDatabase(dataConfig.MongoDatabaseName);

                var siteRepository = new ItemRepositoryMongo(database, new MemorySession<ItemDataModelBase>(),
                    azureBlobSource, new CacheRepository(dataConfig));
                var siteDataModels = await GetSitesCommand.GetSiteDataModelsAsync(siteRepository);

                foreach (var siteDataModel in siteDataModels)
                {
                    try { 
                    //var list = mongoBlob.Downloads("227aefdb-a2b9-4c27-98d9-2f0db43f99ca");
                    var list = await azureBlobSource.DownloadsAsync(siteDataModel.Id);

                    foreach (var fileDataModel in list)
                    {
                        Logger.Default.Info(fileDataModel.SiteId + "-" + fileDataModel.ParentId + "-" +
                                            fileDataModel.PropertyName +
                                            "-" + fileDataModel.FileData.FileName);
                        await azureBlobDestination.UploadAsync(fileDataModel, true);
                        fileDataModel.FileData.Stream.Close();
                        fileDataModel.FileData.Stream.Dispose();
                    }
                        Console.WriteLine("OK");
                }
            catch (Exception ex)
            {
                Logger.Default.Error(ex, "Erreur site");
            }
        }
            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex, "Erreur global batch");
            }
        }*/
    }
}
