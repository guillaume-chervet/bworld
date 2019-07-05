using Demo.User.Identity;
using Demo.User.Site;
using Demo.User.SiteData;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.User
{
    public static class ConfigureExtention
    {
        
        
        public static void ConfigureUserData(this IServiceCollection services)
        {
                services.AddTransient<UserDataService, UserDataService>();
                services.AddTransient<SiteUserService, SiteUserService>();
                services.AddTransient<UserService, UserService>();
                services.AddTransient<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
        }
    }
}