using Demo.Data.History;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.History.Core
{
    public static class ConfigureExtention
    {
        public static void ConfigureHistory(this IServiceCollection services)
        {
            //services.AddTransient<GetGeoCommand, GetGeoCommand>();
            services.ConfigureHistoryData();
        }
    }
}