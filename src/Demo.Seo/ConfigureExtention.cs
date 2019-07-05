using Microsoft.Extensions.DependencyInjection;

namespace Demo.Seo
{
    public static class ConfigureExtention
    {
        public static void ConfigureSeo(this IServiceCollection services)
        {
            services.AddTransient<SeoService, SeoService>();
            services.AddTransient<BddMongo, BddMongo>();
        }
    }
}