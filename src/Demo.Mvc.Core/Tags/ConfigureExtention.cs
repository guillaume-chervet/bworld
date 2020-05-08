using Demo.Mvc.Core.Tags.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Tags
{
    public static class ConfigureExtention
    {
        public static void ConfigureTagsCore(this IServiceCollection services)
        {
            services.AddTransient<GetTagsCommand, GetTagsCommand>();
            services.AddTransient<SaveTagsCommand, SaveTagsCommand>();
            services.ConfigureTagsData();
        }
    }
}