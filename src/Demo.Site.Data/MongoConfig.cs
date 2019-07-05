using System.Linq;
using Demo.Data.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDatabase = MongoDB.Driver.MongoDatabase;

namespace Demo.Data
{
    public static class MongoConfig
    {
        public static void InitIndex()
        {
            var dataConfig = new DataConfig();
            var client = new MongoClient(dataConfig.ConnectionString);
            var mongoServer = client.GetServer();
            var database = mongoServer.GetDatabase(dataConfig.DatabaseName);


            Items(database);
            Seo(database);
            Log(database);
            Cache(database);
            Stats(database);
            Users(database);
            FsFiles(database);
        }

        private static void Users(MongoDatabase database)
        {
            var usersCollection = database.GetCollection("site.user");
            var userIndexes = usersCollection.GetIndexes();

            if (userIndexes.All(c => c.Name != "UserLogins"))
            {
                var keys = IndexKeys.Ascending("Logins.LoginProvider", "Logins.ProviderKey");
                var options = IndexOptions.SetName("Logins");
                usersCollection.CreateIndex(keys, options);
            }
        }

        private static void FsFiles(MongoDatabase database)
        {
            var usersCollection = database.GetCollection("fs.files");
            var userIndexes = usersCollection.GetIndexes();

            if (userIndexes.All(c => c.Name != "SiteId"))
            {
                var keys = IndexKeys.Ascending("metadata.id", "metadata.type");
                var options = IndexOptions.SetName("SiteId");
                usersCollection.CreateIndex(keys, options);
            }
        }

        private static void Stats(MongoDatabase database)
        {
            var statsCollection = database.GetCollection("site.stats");
            var statIndexes = statsCollection.GetIndexes();

            if (statIndexes.All(c => c.Name != "SiteId"))
            {
                var keys = IndexKeys.Ascending("SiteId");
                var options = IndexOptions.SetName("SiteId");
                statsCollection.CreateIndex(keys, options);
            }

            if (statIndexes.All(c => c.Name != "CreateDate"))
            {
                var keys = IndexKeys.Ascending("CreateDate");
                var options = IndexOptions.SetName("CreateDate");
                statsCollection.CreateIndex(keys, options);
            }
        }

        private static void Cache(MongoDatabase database)
        {
            var cacheCollection = database.GetCollection("site.cache");
            var cacheIndexes = cacheCollection.GetIndexes();

            if (cacheIndexes.All(c => c.Name != "SiteId"))
            {
                var keys = IndexKeys.Ascending("SiteId");
                var options = IndexOptions.SetName("SiteId");
                cacheCollection.CreateIndex(keys, options);
            }

            if (cacheIndexes.All(c => c.Name != "ParentId"))
            {
                var keys = IndexKeys.Ascending("Key", "Type");
                var options = IndexOptions.SetName("KeyType");
                cacheCollection.CreateIndex(keys, options);
            }
        }

        private static void Log(MongoDatabase database)
        {
            var createOptions = CollectionOptions
                .SetCapped(true)
                .SetMaxSize(5000)
                .SetMaxDocuments(10000);
            createOptions.SetCapped(true);
            createOptions.SetMaxDocuments(5000);
            createOptions.SetMaxSize(10000);


            if (!database.CollectionExists("site.log"))
                database.CreateCollection("site.log", createOptions);

            var logCollection = database.GetCollection("site.log");
            var logIndexes = logCollection.GetIndexes();

            if (logIndexes.All(c => c.Name != "CreateDate"))
            {
                var keys = IndexKeys.Ascending("CreateDate");
                var options = IndexOptions.SetName("CreateDate");
                logCollection.CreateIndex(keys, options);
            }
        }

        private static void Seo(MongoDatabase database)
        {
            var seoCollection = database.GetCollection("site.seo");
            var seoIndexes = seoCollection.GetIndexes();

            if (seoIndexes.All(c => c.Name != "Url"))
            {
                var keys = IndexKeys.Ascending("Url");
                var options = IndexOptions.SetName("Url");
                seoCollection.CreateIndex(keys, options);
            }

            if (seoIndexes.All(c => c.Name != "SiteId"))
            {
                var keys = IndexKeys.Ascending("SiteId");
                var options = IndexOptions.SetName("SiteId");
                seoCollection.CreateIndex(keys, options);
            }
        }

        private static void Items(MongoDatabase database)
        {
            var itemsCollection = database.GetCollection("site.item");
            var itemIndexes = itemsCollection.GetIndexes();

            if (itemIndexes.All(c => c.Name != "SiteId"))
            {
                var keys = IndexKeys.Ascending("SiteId");
                var options = IndexOptions.SetName("SiteId");
                itemsCollection.CreateIndex(keys, options);
            }

            if (itemIndexes.All(c => c.Name != "ParentId"))
            {
                var keys = IndexKeys.Ascending("SiteId", "ParentId");
                var options = IndexOptions.SetName("ParentId");
                itemsCollection.CreateIndex(keys, options);
            }
        }
    }
}