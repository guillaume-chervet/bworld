using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Stats.Data
{
    public static class ConfigureExtention
    {
        public static void ConfigureStatsData(this IServiceCollection services)
        {
            services.AddTransient<IStatService, StatServiceMongo>();
        }
    }
}