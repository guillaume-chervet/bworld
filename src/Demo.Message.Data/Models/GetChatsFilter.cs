namespace Demo.Data.Message.Models
{
    public class GetChatsFilter : CountChatFilters
    {
        public int Limit { get; set; } = 100;
    }
}