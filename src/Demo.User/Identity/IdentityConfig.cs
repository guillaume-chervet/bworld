/*using System;
using System.Threading.Tasks;
using AspNet.Identity.MongoDB;
using Demo.Email;
using Demo.Log;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MongoDB.Driver;
using Demo.Data.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.MongoDB;
using Microsoft.Extensions.Options;

namespace Demo.User.Identity
{
    // Configurer l'application que le gestionnaire des utilisateurs a utilisée dans cette application. UserManager est défini dans ASP.NET Identity et est utilisé par l'application.

    /*public class ApplicationUserManager : UserManager<ApplicationUser>, IUserManager
    {
        private readonly DataConfig _dataConfig;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IOptions<DataConfig> dataConfig)
            : base(store)
        {
            _dataConfig = dataConfig.Value;
        }

        public ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var client = new MongoClient(_dataConfig.MongoConnectionString);
            var database = client.GetDatabase(_dataConfig.MongoDatabaseName);
            var users = database.GetCollection<ApplicationUser>("site.user");

            //var contextUsers = new IdentityContext(users);
            var store = new UserStore<ApplicationUser>(users);

            /* var container = new UnityContainer();

            var accountInjectionConstructor = new InjectionConstructor(new ApplicationUserManager(store));
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(accountInjectionConstructor);
            container.RegisterType<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>(accountInjectionConstructor);
            */
/*
            var manager = new ApplicationUserManager(store);
            // Configurer la logique de validation pour les noms d'utilisateur
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configurer la logique de validation pour les mots de passe
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };
            // Inscrire les fournisseurs d'authentification à 2 facteurs. Cette application utilise le téléphone et les e-mails comme procédure de réception de code pour confirmer l'utilisateur
            // Vous pouvez indiquer votre propre fournisseur et vous connecter ici.
            manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is: {0}"
            });
            manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Code de sécurité",
                BodyFormat = "Your security code is: {0}"
            });
            manager.EmailService = new EmailService(new EmailConfig(), Logger.Default);
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>
                        (dataProtectionProvider.Create("ASP.NET Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(3)
                    };
            }
            return manager;
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Indiquez votre service de sms ici pour envoyer un message texte.
            return Task.FromResult(0);
        }
    }
}*/