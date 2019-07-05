using Demo.Business.Command.Geo;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Geo.Core
{
    public static class ConfigureExtention
    {
        public static void ConfigureGeoCore(this IServiceCollection services)
        {
            services.AddTransient<GetGeoCommand, GetGeoCommand>();
            services.ConfigureGeoData();
        }
    }
}