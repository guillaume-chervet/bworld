using Demo.Mvc.Core.Sites.Data.Azure;
using Demo.Mvc.Core.Sites.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Sites.Data
{
    public static class ConfigureExtention
    {
            public static void ConfigureSiteData(this IServiceCollection services, IConfiguration Configuration)
            {
            var blobType = Configuration.GetValue<string>("BlobType");

            services.AddTransient<IDataFactory, DataFactoryMongo>();

            if (blobType == "MongoDb")
            {
                services.AddTransient<IDataBlob, MongoBlob>();
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