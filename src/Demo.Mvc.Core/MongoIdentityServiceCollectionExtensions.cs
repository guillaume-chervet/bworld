using System;
using Demo.User.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Driver;
using IdentityRole = Microsoft.AspNetCore.Identity.MongoDB.IdentityRole;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;

namespace Demo.Mvc.Core
{
    public static class MongoIdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddIdentity<TUser>(this IServiceCollection services, IConfiguration configuration)
            where TUser : IdentityUser, new()
        {
            var connectionString = configuration.GetValue<string>("MongoDb:ConnectionString");
            var databaseName = configuration.GetValue<string>("MongoDb:DatabaseName");

            if (connectionString.Contains("?") && connectionString.Contains("/"))
                services.RegisterMongoStores<ApplicationUser, IdentityRole>(connectionString);
            else
                services.RegisterMongoStores<ApplicationUser, IdentityRole>($"{connectionString}/{databaseName}");
            // Services used by identity
            var authentication = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });

            var facebookId = configuration["Authentication:Facebook:AppId"];

            if (!string.IsNullOrEmpty(facebookId)) {
                authentication.AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = facebookId;
                    facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
                });
            }

            var twitterId = configuration["Authentication:Twitter:ConsumerKey"];
            if (!string.IsNullOrEmpty(twitterId))
            {
                authentication.AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = twitterId;
                    twitterOptions.ConsumerSecret = configuration["Authentication:Twitter:ConsumerSecret"];
                });
            }

            var googleId = configuration["Authentication:Google:ClientId"];
            if (!string.IsNullOrEmpty(googleId)) {
                authentication.AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = googleId;
                    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });
            }

            var microsoftId = configuration["Authentication:Microsoft:ClientId"];
            if (!string.IsNullOrEmpty(microsoftId))
            {
                authentication.AddMicrosoftAccount(microsoftOptions =>
                {
                    microsoftOptions.ClientId = microsoftId;
                    microsoftOptions.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
                });
            }
            var cookieDomain = configuration.GetValue<string>("Site:CookieDomain");
            authentication.AddCookie(IdentityConstants.ApplicationScheme, o =>
                {
                    o.LoginPath = new PathString("/Account/Login");
                    o.Events = new CookieAuthenticationEvents
                    {
                        OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                    };
                    o.CookieDomain = cookieDomain;
                    // o.Cookie.SameSite = SameSiteMode.None;
                })
                .AddCookie(IdentityConstants.ExternalScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.ExternalScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    o.CookieDomain = cookieDomain;
                    //  o.Cookie.SameSite = SameSiteMode.None;
                })
                .AddCookie(IdentityConstants.TwoFactorRememberMeScheme,
                    o =>
                    {
                        o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                        o.CookieDomain = cookieDomain;
                        //  o.Cookie.SameSite = SameSiteMode.None;
                    })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    o.CookieDomain = cookieDomain;
                    //    o.Cookie.SameSite = SameSiteMode.None;
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.Domain = cookieDomain;
                options.Cookie.Expiration = TimeSpan.FromDays(7);
                options.LoginPath =
                    "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Identity services
            services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();

            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
            services.TryAddScoped<UserManager<TUser>, AspNetUserManager<TUser>>();
            services.TryAddScoped<SignInManager<TUser>, SignInManager<TUser>>();

            return new IdentityBuilder(typeof(TUser), services);
        }

        public static void RegisterMongoStores<TUser, TRole>(this IServiceCollection services, string connectionString)
            where TUser : IdentityUser where TRole : IdentityRole
        {
            var url = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(url);
            if (url.DatabaseName == null)
                throw new ArgumentException("Your connection string must contain a database name", connectionString);
            var database = mongoClient.GetDatabase(url.DatabaseName, null);
            services.RegisterMongoStores(p => database.GetCollection<TUser>("site.user", null),
                (Func<IServiceProvider, IMongoCollection<TRole>>) (p => database.GetCollection<TRole>("roles", null)));
        }

        public static void RegisterMongoStores<TUser, TRole>(this IServiceCollection services,
            Func<IServiceProvider, IMongoCollection<TUser>> usersCollectionFactory,
            Func<IServiceProvider, IMongoCollection<TRole>> rolesCollectionFactory)
            where TUser : IdentityUser where TRole : IdentityRole
        {
            services.AddSingleton(p => (IUserStore<TUser>) new UserStore<TUser>(usersCollectionFactory(p)));
            services.AddSingleton(p => (IRoleStore<TRole>) new RoleStore<TRole>(rolesCollectionFactory(p)));
        }
    }
}