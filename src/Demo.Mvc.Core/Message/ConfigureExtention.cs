using Demo.Business.Command.Contact.Message;
using Demo.Message.Core.ListMessage;
using Demo.Mvc.Core.Message.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Message
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