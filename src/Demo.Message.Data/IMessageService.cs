using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Message.Models;

namespace Demo.Data.Message
{
    public interface IMessageService
    {
        Task SaveChatAsync(ChatDbModel chatDbModel);
        Task<IList<ChatDbModel>> GetChatsAsync(BoxId boxId, GetChatsFilter getChatsFilter);
        Task<ChatDbModel> GetChatAsync(string chatId);
        Task<long> CountChatAsync(BoxId boxId, CountChatFilters itemFilters);
        Task<long> CountUnreadChatAsync(BoxId boxId, BoxId readerBoxId);
        Task SaveMessageAsync(string chatId, MessageDbModel messageDbModel, Read readDbModel=null);
        Task SaveRead(string chatId, Read readDbModel);
    }
}