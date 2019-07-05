using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using MongoDB.Driver;

namespace Demo.Purge
{
    class Program
    {
        static  void Main(string[] args)
        {

            // On list tous les sites
            var MongoConnectionString = "mongodb://localhost:27017";
            var MongoDatabaseName = "test";
            var client = new MongoClient(MongoConnectionString);
            var database = client.GetDatabase(MongoDatabaseName);
            var collection = database.GetCollection<Item>("site.item");
            var siteIds = collection.AsQueryable<Item>().Select(e => e.SiteId).Distinct().ToList();

            foreach (var siteId in siteIds)
            {
                /*var task = new ItemRepositoryMongo(database, new MemorySession<ItemDataModelBase>(), new MongoBlob(new Database()), new CacheRepository(new StorageConfig())).GetItemsAsync(null, new ItemFilters { Module = "Site" });
                task.Wait();
                var siteDataModels = task.Result;

                var siteDatamodel = siteDataModels.FirstOrDefault(s => s.Id == siteId);

                // Purge des vieux truc
                {
                    var builder = Builders<Item>.Filter;
                    var filter = builder.Eq(c => c.SiteId, siteId) & builder.Eq(c => c.Module, "ImageData");
                   // collection.DeleteMany(filter);
                }

                if (siteDatamodel == null)
                {
                    // On efface tout
                    var items = collection.AsQueryable<Item>().Where(e => e.SiteId == siteId).ToList();

                    var builder = Builders<Item>.Filter;
                    var filter = builder.Eq(c => c.SiteId, siteId);
                    //collection.DeleteMany(filter);

                }
                else
                {



                }*/

            }

        }
    }
}
