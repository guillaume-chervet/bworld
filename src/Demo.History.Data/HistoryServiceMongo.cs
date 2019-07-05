using System.Threading.Tasks;
using Demo.Data.History.Models;
using MongoDB.Driver;
using Demo.Data.Mongo;

namespace Demo.Data.History
{
    public class HistoryServiceMongo : IHistoryService
    {
        private readonly IMongoCollection<HistoryDbModel> _collection;

        public HistoryServiceMongo(IDatabase db)
        {
            var database = db.GetDatabase();
            _collection = database.GetCollection<HistoryDbModel>("site.histories");
        }

        public async Task AddAsync(string id, CheckPoint checkPoint)
        {
            var update = Builders<HistoryDbModel>.Update
                .Push("CheckPoints", checkPoint);
            var builder = Builders<HistoryDbModel>.Filter;
            var filter = builder.Eq(x => x.ElementId, id);
            await _collection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task<HistoryDbModel> GetHistoryAsync(string id)
        {
            var builder = Builders<HistoryDbModel>.Filter;
            var filter = builder.Eq(p => p.ElementId, id);
            var historyDbModel = (await _collection.FindAsync(filter)).FirstOrDefault();
            return historyDbModel;
        }

    }
}