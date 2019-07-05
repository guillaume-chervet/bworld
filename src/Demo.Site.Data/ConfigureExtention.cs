using Demo.Data.Azure;
using Demo.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Data
{
    public static class ConfigureExtention
    {
            public static void ConfigureSiteData(this IServiceCollection services, IConfiguration Configuration)
            {
            var blobType = Configuration.GetValue<string>("BlobType");

            services.AddTransient<IDataFactory, DataFactoryMongo>();

            if (blobType == "MongoDb")
            {
                services.AddTransient<IDataBlob, AzureBlob>();
            }
            else
            {
                services.AddTransient<IDataBlob, AzureBlob>();
            }
                services.AddTransient<ICacheRepository, CacheRepository>();
                services.AddTransient<IItemRepository, ItemRepositoryMongo>();
            }
    }
}