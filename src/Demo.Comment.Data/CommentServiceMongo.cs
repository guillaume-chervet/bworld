using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Data.Comment.Models;
using MongoDB.Driver;
using Demo.Data.Mongo;
using MongoDB.Bson;

namespace Demo.Data.Comment
{
    public class CommentServiceMongo : ICommentService
    {
        private readonly IMongoCollection<CommentsDbModel> _comments;

        public CommentServiceMongo(IDatabase db)
        {
            var database = db.GetDatabase();
            var collectionSettings = new MongoCollectionSettings {
                GuidRepresentation = GuidRepresentation.CSharpLegacy
            };
            _comments = database.GetCollection<CommentsDbModel>("site.msg.comment", collectionSettings);
        }

        public async Task SaveCommentAsync(string siteId, string moduleId, CommentDbModel commentDbModel)
        {
            SetMessageId(commentDbModel);

            var update = Builders<CommentsDbModel>.Update
                     .Push("Comments", commentDbModel)
                     .Inc("NumberComments", 1);
            var builder = Builders<CommentsDbModel>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Eq(x => x.ModuleId, moduleId);
            await _comments.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = true });
        }

        public async Task DeleteCommentAsync(string siteId, string moduleId, string commentId)
        {
            var update = Builders<CommentsDbModel>.Update
                        .Set("Comments.$.IsDeleted", true)
                        .Inc("NumberComments", -1);
            var builder = Builders<CommentsDbModel>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Eq(x => x.ModuleId, moduleId) & builder.Eq("Comments._id", new Guid(commentId));
            await _comments.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = false });
        }

        public async Task<CommentsDbModel> GetCommentsAsync(string moduleId)
        {
            var builder = Builders<CommentsDbModel>.Filter;
            var filter = builder.Eq(x => x.ModuleId, moduleId);
            return (await _comments.Find(filter).ToListAsync()).FirstOrDefault();
        }

        public async Task<int> CountCommentsAsync(string moduleId)
        {
            var builder = Builders<CommentsDbModel>.Filter;
            var filter = builder.Eq(x => x.ModuleId, moduleId);
            var projection = Builders<CommentsDbModel>.Projection.Expression(x => new { NumberComments = x.NumberComments });

            var cursor = await _comments.Find(filter).Project(projection).ToListAsync();

            foreach (var commentsDbModel in cursor)
            {
                return commentsDbModel.NumberComments;
            }

            return 0;
        }

        private static void SetMessageId(CommentDbModel commentDbModel)
        {
            if (string.IsNullOrEmpty(commentDbModel.Id))
            {
                commentDbModel.Id = Guid.NewGuid().ToString();
            }
        }

    }
}