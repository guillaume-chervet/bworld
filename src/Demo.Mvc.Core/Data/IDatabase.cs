namespace Demo.Mvc.Core.Data
{
    public interface IDatabase
    {
        MongoDB.Driver.IMongoDatabase GetDatabase();
    }
}