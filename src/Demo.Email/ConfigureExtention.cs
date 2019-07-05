using Microsoft.Extensions.DependencyInjection;

namespace Demo.Email
{
    public static class ConfigureExtention
    {
        public static void ConfigureMail(this IServiceCollection services, bool isDevelopment)
        {
            if (isDevelopment)
            {
                services.AddTransient<IEmailService, EmailConsoleService>();
            }
            else
            {
                services.AddTransient<IEmailService, EmailSendGridService>();
            }
        }
    }
}