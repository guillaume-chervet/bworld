using Demo.Business.Command.Stats;
using Demo.Stats.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Stats.Core
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