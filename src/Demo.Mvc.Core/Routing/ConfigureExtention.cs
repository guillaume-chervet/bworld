using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Routing
{
    public static class ConfigureExtention
    {
        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddTransient<IRouteManager, RouteManager>();
        }
    }
}