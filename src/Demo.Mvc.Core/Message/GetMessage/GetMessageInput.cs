using Demo.Data.Message.Models;

namespace Demo.Business.Command.Contact.Message.Models
{
    public class GetMessageInput
    {
        public BoxId BoxId { get; set; }
        public string ChatId { get; set; }
    }
}