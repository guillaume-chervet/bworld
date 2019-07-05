using MongoDB.Driver;

namespace Demo.Data.Mongo
{
    public interface IDatabase
    {
        MongoDB.Driver.IMongoDatabase GetDatabase();
    }
}