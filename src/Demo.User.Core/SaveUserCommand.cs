using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.User.Models;
using Demo.Business.Renderers;
using Demo.Common.Command;
using Demo.Email;
using Demo.Renderer;
using Demo.Routing.Extentions;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Site;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Demo.Common;
using System.IO;

namespace Demo.Business.Command.User
{
    public class SaveUserCommand : Command<UserInput<SaveUserInput>, CommandResult>
    {
        private readonly UserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
 

        public SaveUserCommand(UserService userService, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _userService = userService;
            _userManager = userManager;
            _emailService = emailService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            
            if (Input.UserId != Input.Data.UserId)
            {
              await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);
            }

            var userId = Input.Data.UserId;

            var applicationUser = await _userService.FindApplicationUserByIdAsync(Input.Data.UserId);

            // TODO validation des données
            if (!string.IsNullOrEmpty(Input.Data.Email))
            {
                var applicationUserEmail = await _userService.FindApplicationUserByEmailAsync(Input.Data.Email);
                if (applicationUserEmail != null)
                {
                    Result.ValidationResult.AddError("EMAIL_ALREADY_EXIST");
                    return;
                }

                if ( Input.Data.Email.ToLower() != applicationUser.Email.ToLower())
                {
                    applicationUser.Email = Input.Data.Email.ToLower().Trim();
                    applicationUser.EmailConfirmed = false;

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                    var callbackUrl = "https://www.bworld.fr/Account/ConfirmEmail?userId={0}&code={1}";

                    var createUserMailModel = new UserChangeMailModel();
                    createUserMailModel.CallbackUrl = string.Format(callbackUrl, WebUtility.UrlEncode(userId),
                        WebUtility.UrlEncode(code));
                    createUserMailModel.UserName = applicationUser.FullName;

                    var templateMail =
                        new StringTemplateRenderer().Render( ResourcesLoader.Load(Path.Combine( "Renderers" ,"UserChangeMail.st")),
                            createUserMailModel);
                    await
                        SendEmailAsync(_emailService, applicationUser, "[bworld] Confirmation de votre email", templateMail);
                }
            }
            else
            {
                applicationUser.FirstName = Input.Data.FirstName;
                applicationUser.LastName = Input.Data.LastName;
                applicationUser.AuthorUrl = Input.Data.AuthorUrl;
            }

            await _userService.SaveAsync(applicationUser);
        }

        public static async Task SendConfirmEmailAsync(IEmailService emailService, UserManager<ApplicationUser> userManager, SendConfirmEmailModel model, ApplicationUser user)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
            const string callbackUrl = "https://www.bworld.fr/Account/ConfirmEmail?userId={0}&code={1}";

            var createUserMailModel = new CreateUserMailModel();
            createUserMailModel.CallbackUrl = string.Format(callbackUrl, WebUtility.UrlEncode(user.Id), WebUtility.UrlEncode(code));
            createUserMailModel.UserName = user.FullName;
            createUserMailModel.Provider = model.Provider;

            var templateMail =
                new StringTemplateRenderer().Render(ResourcesLoader.Load(Path.Combine("Renderers", "CreateUser.st")),
                    createUserMailModel);
            await SendEmailAsync(emailService, user, "[bworld] Confirmation de votre email", templateMail);
        }

        public static async Task AttachRolesAsync(UserService userService, SiteUserService siteUserService, string userId)
        {
            var user = await userService.FindApplicationUserByIdAsync(userId);
            var siteUsers = await siteUserService.FindUsersByEmailAsync(user.Email);
            foreach (var siteUserDbModel in siteUsers)
            {
                if (siteUserDbModel.FlaggedRoles!=null)
                {
                    foreach (var siteUserRole in siteUserDbModel.FlaggedRoles)
                    {
                            string role = UserSecurity.MapRole(siteUserDbModel.SiteId, siteUserRole);
                            if (user.Roles.Count(c => c ==role)<=0)
                            {
                                user.Roles.Add(role);
                            }
                    }

                }
            }
            await userService.SaveAsync(user);
        }

        public static async Task SendResetPasswordEmailAsync(IEmailService emailService, ApplicationUser user, string token)
        {
            const string callbackUrl = "https://www.bworld.fr/utilisateur/reinit-password?userId={0}&token={1}&dm=false";

            var createUserMailModel = new ResetPasswordMailModel();
            createUserMailModel.CallbackUrl = string.Format(callbackUrl, WebUtility.UrlEncode(user.Id), WebUtility.UrlEncode(token));
            createUserMailModel.UserName = user.FullName;

            var templateMail =
                new StringTemplateRenderer().Render(ResourcesLoader.Load(Path.Combine("Renderers", "ResetPassword.st")), createUserMailModel);
            await SendEmailAsync(emailService, user, "[bworld] Re-initialisation mot de votre mot passe", templateMail);
        }

        private static async Task  SendEmailAsync(IEmailService emailService, ApplicationUser user, string subject, string body)
        {
            await emailService.SendAsync(new MailMessage()
            {
                Body = body,
                Subject = subject,
                Destination = user.Email
            });
        }

    }
}