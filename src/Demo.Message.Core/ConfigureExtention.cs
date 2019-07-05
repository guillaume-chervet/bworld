using Demo.Data.Message;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Business.Command.Contact.Message
{
    public static class ConfigureExtention
    {
            public static void ConfigureMessageCore(this IServiceCollection services)
            {
                services.AddTransient<GetMessageCommand, GetMessageCommand>();
                services.AddTransient<ListMessageCommand, ListMessageCommand>();
                services.AddTransient<SendMessageCommand, SendMessageCommand>();
                services.ConfigureMessageData();
            }
    }
}