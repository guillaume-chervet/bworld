using Demo.Data.Tags;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Stats.Core
{
    public static class ConfigureExtention
    {
        public static void ConfigureTagsData(this IServiceCollection services)
        {
            services.AddTransient<TagsServiceMongo, TagsServiceMongo>();
        }
    }
}