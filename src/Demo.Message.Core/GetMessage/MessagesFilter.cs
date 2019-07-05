using System;

namespace Demo.Business.Command.Contact.Message.Models
{
    public class MessagesFilter
    {
        public DateTime? Date { get; set; }
        public bool IsPrevious { get; set; }
    }
}