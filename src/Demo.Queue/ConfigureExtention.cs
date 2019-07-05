using Microsoft.Extensions.DependencyInjection;

namespace Demo.Queue
{
    public static class ConfigureExtention
    {
        public static void ConfigureQueue(this IServiceCollection services)
        {
            services.AddTransient<MemoryQueue, MemoryQueue>();
        }
    }
}