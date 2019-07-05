using Demo.Data.Message.Models;

namespace Demo.Business.Command.Contact.Message.Models
{
    public class SendMessageInput
    {
        public string ChatId { get; set; }
        public BoxId To { get; set; }
        public BoxId From { get; set; }
        public string Type { get; set; }
        public string Source { get; set; }
        public string MessageJson { get; set; }
    }
}