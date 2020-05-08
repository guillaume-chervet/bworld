using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Data
{
    public static class ConfigureExtention
    {
        public static void ConfigureDataMongo(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<DataConfig>(Configuration.GetSection("MongoDb"));

            services.AddTransient<IDatabase, MongoDatabase>();
        }
    }
}