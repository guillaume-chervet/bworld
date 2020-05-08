using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Message.Models;
using Demo.Mvc.Core.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Demo.Mvc.Core.Message.Data
{
    public class MessageServiceMongo : IMessageService
    {
        private readonly IMongoCollection<ChatDbModel> _chats;

        public MessageServiceMongo(IDatabase db)
        {
            var database = db.GetDatabase();

            _chats = database.GetCollection<ChatDbModel>("site.msg.chat");
        }

        public async Task SaveChatAsync(ChatDbModel chatDbModel)
        {
            if (chatDbModel.Messages != null)
            {
                foreach (var messageDbModel in chatDbModel.Messages)
                {
                    SetMessageId(messageDbModel);
                }
            }

            await _chats.InsertOneAsync(chatDbModel);
        }

        public async Task SaveMessageAsync(string chatId, MessageDbModel messageDbModel, Read readDbModel = null)
        {
            var update = Builders<ChatDbModel>.Update
                       .Set("UpdateDate", messageDbModel.CreateDate)
                       .Inc("NumberMessages", 1)
                       .Push("Messages", messageDbModel);

            if (readDbModel != null)
            {
                update = update.Push("Reads", readDbModel);
                update = update.Push("LastReads", readDbModel);
            }

            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq(x => x.Guid, new Guid(chatId));
            await _chats.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = false });
        }

        public async Task<ChatDbModel> GetChatAsync(string chatId)
        {
            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq(x => x.Guid, new Guid(chatId));
            var messageDbModel = (await _chats.FindAsync(filter)).FirstOrDefault();
            return messageDbModel;
        }

        public async Task<IList<ChatDbModel>> GetChatsAsync(BoxId boxId, GetChatsFilter getChatsFilter)
        {
            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq("To._id._id", boxId.Id) & builder.Eq("To._id.Type", boxId.Type);

            if (getChatsFilter.DateGt.HasValue)
            {
                filter = filter & builder.Gt(p => p.UpdateDate, getChatsFilter.DateGt.Value);
            }

            if (getChatsFilter.DateLt.HasValue)
            {
                filter = filter & builder.Lt(p => p.UpdateDate, getChatsFilter.DateLt.Value);
            }

            var sort = Builders<ChatDbModel>.Sort.Descending(x => x.UpdateDate);
            var cursor = _chats.Find(filter);
            cursor.Sort(sort);

            cursor.Limit(getChatsFilter.Limit);
            var chats = await cursor.ToListAsync();

            // TODO remove
            foreach (var chatDbModel in chats)
            {
                if (chatDbModel.LastReads == null)
                {
                    chatDbModel.LastReads = new List<Read>();
                    await _chats.ReplaceOneAsync(new BsonDocument("_id", new Guid(chatDbModel.Id)), chatDbModel);
                }
            }

            return chats;
        }

        public async Task<long> CountChatAsync(BoxId boxId, CountChatFilters getChatsFilter)
        {
            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq("To._id._id", boxId.Id) & builder.Eq("To._id.Type", boxId.Type); 

            if (getChatsFilter.DateGt.HasValue)
            {
                filter = filter & builder.Gt(p => p.UpdateDate, getChatsFilter.DateGt.Value);
            }

            if (getChatsFilter.DateLt.HasValue)
            {
                filter = filter & builder.Lt(p => p.UpdateDate, getChatsFilter.DateLt.Value);
            }
            var count = await _chats.CountAsync(filter);
            return count;
        }

        public async Task<long> CountUnreadChatAsync(BoxId boxId, BoxId readerBoxId)
        {
            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq("To._id._id", boxId.Id) & builder.Eq("To._id.Type", boxId.Type);

            filter = filter & builder.Ne("LastReads._id._id", readerBoxId.Id); 

            var count = await _chats.CountAsync(filter);
            return count;
      }


      public async Task SaveRead(string chatId, Read readDbModel)
      {

            var update = Builders<ChatDbModel>.Update
                         .Push("LastReads", readDbModel)
                       .Push("Reads", readDbModel);
            var builder = Builders<ChatDbModel>.Filter;
            var filter = builder.Eq(x => x.Guid, new Guid(chatId));
           await _chats.UpdateOneAsync(filter, update, new UpdateOptions() { IsUpsert = false });
        }

        private static void SetMessageId(MessageDbModel messageDbModel)
        {
            if (string.IsNullOrEmpty(messageDbModel.Id))
            {
                messageDbModel.Id = Guid.NewGuid().ToString();
            }
        }

    }
}