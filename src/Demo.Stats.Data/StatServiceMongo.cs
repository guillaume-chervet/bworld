using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Mongo;
using Demo.Data.Stat.Models;
using MongoDB.Driver;

namespace Demo.Data.Stat
{
    public class StatServiceMongo : IStatService
    {
        private readonly IMongoCollection<StatsDbModel> _Collection;

        public StatServiceMongo(IDatabase db)
        {
            var database = db.GetDatabase();
            _Collection = database.GetCollection<StatsDbModel>("site.stats");
        }

        public async Task AddAsync(StatDbModel statDbModel)
        {
            var date = statDbModel.CreateDate.Date.AddHours(4);
            if (string.IsNullOrEmpty(statDbModel.Id))
            {
                statDbModel.Id = Guid.NewGuid().ToString();
            }

            var update = Builders<StatsDbModel>.Update
                .Push("Stats", statDbModel);
            var builder = Builders<StatsDbModel>.Filter;
            var filter = builder.Eq(x => x.SiteId, statDbModel.SiteId) & builder.Eq(x => x.Date, date);
            await _Collection.UpdateOneAsync(filter, update, new UpdateOptions {IsUpsert = true});
        }

        public async Task<IList<StatDbModel>> GetStatsync(DateTime beginDate, DateTime endDate, string siteId)
        {
            var builder = Builders<StatsDbModel>.Filter;
            var filter = builder.Eq(p => p.SiteId, siteId) & builder.Lte(p => p.Date, endDate) &
                         builder.Gte(p => p.Date, beginDate);

            var listBlobs = (await _Collection.FindAsync(filter)).ToList();

            var result = new List<StatDbModel>();
            foreach (var blob in listBlobs) result.AddRange(blob.Stats);

            return result;
        }
    }
}