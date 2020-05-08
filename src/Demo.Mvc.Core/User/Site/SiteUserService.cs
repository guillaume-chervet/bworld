using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Demo.User.Identity;
using Demo.User.Site;
using System;
using Demo.Mvc.Core.Data;

namespace Demo.User.Site
{
    public class SiteUserService
    {
        private readonly IMongoCollection<SiteUserDbModel> _collection;

        public SiteUserService(IDatabase db)
        {
            var database = db.GetDatabase();
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _collection = database.GetCollection<SiteUserDbModel>("site.siteuser", collectionSettings);
        }

        public async Task SaveAsync(SiteUserDbModel user)
        {
            if (!string.IsNullOrEmpty(user.Id))
            {
                await _collection.ReplaceOneAsync(new BsonDocument("_id", new Guid(user.Id)), user);
            }
            else
            {
                await _collection.InsertOneAsync(user);
            }
        }

        public async Task<SiteUserDbModel> FindByEmailAsync(string siteId, string userEmail)
        {
            var builder = Builders<SiteUserDbModel>.Filter;
            var filter = builder.Eq(p => p.Mail, userEmail) & builder.Eq(p => p.SiteId, siteId);
            return (await _collection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IList<SiteUserDbModel>> FindUsersByEmailAsync(string userEmail)
        {
            var builder = Builders<SiteUserDbModel>.Filter;
            var filter = builder.Eq(p => p.Mail, userEmail);
            return (await _collection.FindAsync(filter)).ToList();
        }

        public async Task<SiteUserDbModel> FindAsync(string siteUserId)
        {
            var builder = Builders<SiteUserDbModel>.Filter;
            var filter = builder.Eq(p => p.Guid, new Guid(siteUserId));
            return (await _collection.FindAsync(filter)).FirstOrDefault();
        }

        public async Task<IList<SiteUserDbModel>> FindUsersByUserIdAsync(string userId)
        {
            var builder = Builders<SiteUserDbModel>.Filter;
            var filter = builder.Eq(p => p.UserId, userId);
            return (await _collection.FindAsync(filter)).ToList();
        }

        public async Task<IList<SiteUserDbModel>> FindBySiteId(string siteId)
        {
            var builder = Builders<SiteUserDbModel>.Filter;
            var filter = builder.Eq(p => p.SiteId, siteId);
            return (await _collection.FindAsync(filter)).ToList();
        }

    }
}