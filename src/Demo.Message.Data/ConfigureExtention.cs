using Microsoft.Extensions.DependencyInjection;

namespace Demo.Data.Message
{
    public static class ConfigureExtention
    {
            public static void ConfigureMessageData(this IServiceCollection services)
            {
                services.AddTransient<IMessageService, MessageServiceMongo>();
            }
    }
}