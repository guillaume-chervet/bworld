using System;
using System.Collections.Generic;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    public class ChatItem
    {
        public string Title { get; set; }
        public ContactItem From { get; set; }
        public IList<ContactItem> To { get; set; }
        public IList<MessageItem> Messages { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public bool? Readed { get; set; }
        public string Id { get; set; }
    }
}