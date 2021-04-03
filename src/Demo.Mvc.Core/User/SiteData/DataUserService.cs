using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.User.SiteData.Model;
using MongoDB.Driver;
using Demo.Mvc.Core.Data;
using MongoDB.Bson;

namespace Demo.User.SiteData
{
    public class UserDataService
    {
        private readonly IMongoCollection<UserDataDbModel> _collection;

        public UserDataService(IDatabase db)
        {
            var database = db.GetDatabase();
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _collection = database.GetCollection<UserDataDbModel>("site.user.data", collectionSettings);
        }

        public async Task<string> SaveAsync(UserDataDbModel userDataDbModel)
        {
            var builder = Builders<UserDataDbModel>.Filter;
            string siteId = userDataDbModel.SiteId;
            string userId = userDataDbModel.UserId;
            if (string.IsNullOrEmpty(userDataDbModel.Id))
            {
                userDataDbModel.Guid = Guid.NewGuid();
                await _collection.InsertOneAsync(userDataDbModel);
            }
            else
            {
                var update = Builders<UserDataDbModel>.Update
                    .Set(d => d.EndTicks, userDataDbModel.EndTicks)
                    .Set(d => d.CookieSessionId, userDataDbModel.CookieSessionId)
                    .Set(d => d.Json, userDataDbModel.Json);
                var query = builder.Eq(x => x.SiteId, siteId)
                            & builder.Eq(x => x.UserId, userId)
                            & builder.Eq(x => x.Guid, userDataDbModel.Guid)
                            & builder.Eq(x => x.ElementId, userDataDbModel.ElementId)
                            & builder.Eq(x => x.ModuleId, userDataDbModel.ModuleId);
                await _collection.UpdateOneAsync(query, update, new UpdateOptions() { IsUpsert = false });
            }
           return userDataDbModel.Id;
        }

        public async Task<IList<UserDataDbModel>> GetAsync(string siteId, string userId, string cookieSessionId=null)
        {
            var builder = Builders<UserDataDbModel>.Filter;
            var query = builder.Eq(x => x.SiteId, siteId);
            if (!string.IsNullOrEmpty(userId))
            {
                query = query & builder.Eq(x => x.UserId, userId);
            }
            else
            {
                query = query & builder.Eq(x => x.CookieSessionId, cookieSessionId);
            }
                        
            var cursor = _collection.Find(query);

            cursor.Sort(Builders<UserDataDbModel>.Sort.Descending(x => x.BeginTicks));
            return await cursor.ToListAsync();
        }
    }
}