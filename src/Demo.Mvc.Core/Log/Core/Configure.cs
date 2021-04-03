using Microsoft.Extensions.DependencyInjection;

namespace Demo.Log.Core
{
    public static class ConfigureExtention
    {
        public static void ConfigureLogCore(this IServiceCollection services)
        {
            services.AddTransient<ClearLogCommand, ClearLogCommand>();
            services.AddTransient<GetLogCommand, GetLogCommand>();

            Demo.Log.Configure.ConfigureServices(services);
        }
    }
}