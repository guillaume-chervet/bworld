using Demo.Data.Message.Models;

namespace Demo.Business.Command.Contact.Message.Models.ListMessage
{
    public class ContactItem
    {
        public string FullName { get; set; }
        public string Id { get; set; }
        public TypeBox Type { get; set; }
    }
}