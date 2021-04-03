using Demo.Mvc.Core.Geo.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Geo.Core
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