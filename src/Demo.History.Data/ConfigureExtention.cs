using Microsoft.Extensions.DependencyInjection;

namespace Demo.Data.History
{
    public static class ConfigureExtention
    {
        public static void ConfigureHistoryData(this IServiceCollection services)
        {
            services.AddTransient<IHistoryService, HistoryServiceMongo>();
        }
    }
}