using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Tags.Data
{
    public static class ConfigureExtention
    {
        public static void ConfigureTagsData(this IServiceCollection services)
        {
            services.AddTransient<TagsServiceMongo, TagsServiceMongo>();
        }
    }
}