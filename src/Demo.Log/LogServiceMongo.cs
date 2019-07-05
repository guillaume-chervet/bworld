using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Mongo;
using MongoDB.Driver;

namespace Demo.Log
{
    public class LogServiceMongo : ILogService
    {
        private readonly IMongoCollection<Log> _collection;
        private const string CollectionName = "site.logs";
        public LogServiceMongo(IDatabase db)
        {
            var database = db.GetDatabase();
            _collection = database.GetCollection<Log>(CollectionName);
        }

        public async Task ClearLogsAsync()
        {
            await _collection.Database.DropCollectionAsync(CollectionName);
        }

        public async Task<IList<Log>> GetLogs(GetLogsInput getLogsInput = null)
        {

            if (getLogsInput == null)
            {
                throw new ArgumentException("getLogsInput is null or empty");
            }

            var beginDate = getLogsInput.BeginDate ?? DateTime.Now.AddHours(-6);
            var endDate = getLogsInput.EndDate ?? DateTime.Now.AddHours(3);

            /*var query = Query.And(
                Query<Log>.GT(p => p.CreateDate, beginDate),
                Query<Log>.LT(p => p.CreateDate, endDate));*/

            var builder = Builders<Log>.Filter;

            var filter = builder.Empty;
            filter = filter & builder.Gt(x => x.CreateAt, new DateTimeOffset(beginDate)) & builder.Lt(x => x.CreateAt, new DateTimeOffset(endDate));

            if (getLogsInput.Level.HasValue)
            {
                filter = filter & builder.Eq(p => p.Level, getLogsInput.Level.Value);
            }

            if ( !string.IsNullOrEmpty(getLogsInput.Origin))
            {
                filter = filter & builder.Eq(p => p.ApplicationName, getLogsInput.Origin);
            }

            if (!string.IsNullOrEmpty(getLogsInput.Filter))
            {
                filter = filter & builder.Where(l => l.Message.ToLower().Contains(getLogsInput.Filter)) | builder.Where(l => l.Exception.ToLower().Contains(getLogsInput.Filter));
            }

            var cursor = _collection.Find(filter);

            cursor.Limit(getLogsInput.Limit ?? 1000);

            var sort = Builders<Log>.Sort.Descending(x => x.CreateAt);

            var logs = await cursor.Sort(sort).ToListAsync();
            return logs;
        }

    }
}