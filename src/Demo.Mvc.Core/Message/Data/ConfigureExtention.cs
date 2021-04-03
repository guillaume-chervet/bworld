using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Message.Data
{
    public static class ConfigureExtention
    {
            public static void ConfigureMessageData(this IServiceCollection services)
            {
                services.AddTransient<IMessageService, MessageServiceMongo>();
            }
    }
}