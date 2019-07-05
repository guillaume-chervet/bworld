using Demo.Data.Message.Models;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    public class ListMessageInput
    {
        public BoxId BoxId { get; set; }
        public MessagesFilter Filter { get; set; }
    }
}