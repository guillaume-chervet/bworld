using Demo.Data.Stat;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Stats.Data
{
    public static class ConfigureExtention
    {
        public static void ConfigureStatsData(this IServiceCollection services)
        {
            services.AddTransient<IStatService, StatServiceMongo>();
        }
    }
}