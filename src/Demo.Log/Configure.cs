using Microsoft.Extensions.DependencyInjection;

namespace Demo.Log
{
    public class Configure
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogService,LogServiceMongo>();
        }
    }
}