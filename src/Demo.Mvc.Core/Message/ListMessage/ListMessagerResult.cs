using System.Collections.Generic;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    public class ListMessageResult : PaginationResult
    {
        public IList<ChatItem> Chats { get; set; }
    }
}