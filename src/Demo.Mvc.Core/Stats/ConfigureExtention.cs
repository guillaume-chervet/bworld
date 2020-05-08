using Demo.Mvc.Core.Stats.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Stats
{
    public static class ConfigureExtention
    {
        public static void ConfigureStatsCore(this IServiceCollection services)
        {
            services.AddTransient<GetStatsCommand, GetStatsCommand>();
            services.AddTransient<SaveStatsCommand, SaveStatsCommand>();
            services.ConfigureStatsData();
        }
    }
}