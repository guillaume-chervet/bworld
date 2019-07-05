using System;
using System.Globalization;
using System.Linq;
using Demo.Data.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Demo.Data.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void InitData()
        {
            using (var dataFactory = new DataFactoryMock())
            {
                var cultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures);

                foreach (var cultureInfo in cultureInfos)
                {
                }
            }
        }

        [TestMethod]
        public void MongoTest()
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var server = client.GetServer();

            var database = server.GetDatabase("myworld"); // "test" is the name of the database

            // "entities" is the name of the collection
            var collection = database.GetCollection<Entity>("entities");

            var entity = new Entity {Name = "Tom"};
            collection.Insert(entity);
            var id = entity.Id; // Insert will set the Id if necessary (as it was in this example)


            var query = Query<Entity>.EQ(e => e.Id, id);
            var entity1 = collection.FindOne(query);

            entity.Name = "Dick";
            collection.Save(entity);
        }

        [TestMethod]
        public void request()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();

            var database = server.GetDatabase("myworld"); // "test" is the name of the database

            var collection = database.GetCollection<Item>("site.item");

            var query2 = Query<Item>.Where(c => c.CreateDate > DateTime.MinValue);
            var items2 = collection.Find(query2).ToList();

            /* var query = Query.And(
             Query<Item>.EQ(p => p.SiteId, null),
                                 Query<Item>.EQ(p => p.ParentId, null)
                               //  Query<Item>.EQ(p => p.Module, "Site")
                                 );*/


            var query = Query<Item>.Where(c => c.IsTemporary == false);
            var items = collection.Find(query2).ToList();
        }

        [TestMethod]
        public void ClearItem()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var server = client.GetServer();

            var database = server.GetDatabase("myworld"); // "test" is the name of the database

            // "entities" is the name of the collection
            var collection = database.GetCollection<Entity>("site.item");
            var query = Query<Item>.Where(c => c.CreateDate >= DateTime.MinValue);
            var result = collection.Remove(query);


            var collection2 = database.GetCollection<IdentityUser>("site.user");
            var query2 = Query<IdentityUser>.Where(c => c.EmailConfirmed == false);
            var result2 = collection2.Remove(query2);
        }

        [TestMethod]
        public void ClearChacheItem()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var server = client.GetServer();

            var database = server.GetDatabase("myworld"); // "test" is the name of the database

            // "entities" is the name of the collection
            var collection = database.GetCollection<Entity>("site.item");
            var query = Query<Item>.Where(c => c.CreateDate >= DateTime.MinValue);
            var result = collection.Remove(query);


            var collection2 = database.GetCollection<IdentityUser>("site.user");
            var query2 = Query<IdentityUser>.Where(c => c.EmailConfirmed == false);
            var result2 = collection2.Remove(query2);
        }

        /*    [TestMethod]
        public void DataFactoryMongo()
        {

            var dataFactoryMongo = new DataFactoryMongo(new DataConfig());

            var size =  dataFactoryMongo.ItemRepository.CountSiteSizeBytes("c27e39ee-7ba9-46f8-aa7c-9e334c72a96c");

            //http://www.convertworld.com/fr/mesures-informatiques/Megaoctet+%28M%C3%A9gabyte%29.html
             var mega = size/1048576;
             var kilo = (size - mega*1048576)/1024;


        }*/

        [TestMethod]
        public void TransertSiteData()
        {
            const string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            var server = client.GetServer();

            var database = server.GetDatabase("myworld"); // "test" is the name of the database

            // "entities" is the name of the collection
            var collection = database.GetCollection<Item>("site.item");
            var query = Query<Item>.Where(c => c.CreateDate >= DateTime.MinValue);

            var items = collection.FindAll();
            foreach (var item in items)
            {
            }
        }

        public class Entity
        {
            // public ObjectId Id { get; set; }


            [BsonId(IdGenerator = typeof (CombGuidGenerator))]
            public Guid Id { get; set; }

            [BsonElement("Name")]
            public string Name { get; set; }
        }
    }
}