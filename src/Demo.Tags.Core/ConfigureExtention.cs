using Demo.Business.Command.Tags;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Stats.Core
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