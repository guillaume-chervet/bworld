using System;
using System.Collections.Generic;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    public class MessageItem
    {
        public string Title { get; set; }
        public ContactItem From { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastMessageDate { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
    }
}