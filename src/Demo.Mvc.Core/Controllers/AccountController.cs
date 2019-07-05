using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command.User;
using Demo.Business.Command.User.Models;
using Demo.Common;
using Demo.Common.Command;
using Demo.Email;
using Demo.Mvc.Core.Controllers.Models;
using Demo.User.Identity;
using Demo.User.Site;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Controllers
{
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly SiteUserService _siteUserService;

        private readonly UserService _userService;

        //
        // POST: /Account/ExternalLogin
        public AccountController(BusinessFactory business, UserService userService, SiteUserService siteUserService,
            IEmailService emailService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager) : base(business)
        {
            _userService = userService;
            _siteUserService = siteUserService;
            _emailService = emailService;
            UserManager = userManager;
            SignInManager = signInManager;
        }

        protected UserManager<ApplicationUser> UserManager { get; }

        public SignInManager<ApplicationUser> SignInManager { get; }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLoginAssociation(string provider, string returnUrl)
        {
            // Demandez une redirection vers le fournisseur de connexions externe
            var redirectUri = Url.Action("ExternalLoginAssociationCallback", "Account", new {ReturnUrl = returnUrl});
            var properties = new AuthenticationProperties {RedirectUri = redirectUri};

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginAssociationCallback(string returnUrl)
        {
            returnUrl = returnUrl.Replace("://", "----").Replace(":", "---");
            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo != null && User.Identity.IsAuthenticated)
            {
                var loginProvider = loginInfo.LoginProvider;
                // Connecter cet utilisateur à ce fournisseur de connexion externe si l'utilisateur possède déjà une connexion
                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var login = user.Logins.Find(u => u.LoginProvider == loginInfo.LoginProvider);

                    if (login == null)
                    {
                        user.AddLogin(new UserLoginInfo(loginInfo.LoginProvider, loginInfo.ProviderKey,
                            loginInfo.ProviderDisplayName));
                        await UserManager.UpdateAsync(user);

                        returnUrl = returnUrl.Replace("://", "----").Replace(":", "---");
                        return Redirect(string.Format(
                            "/utilisateur/confirmation-association-compte-externe?provider={0}&returnUrl={1}&dm=false",
                            WebUtility.UrlEncode(loginProvider), WebUtility.UrlEncode(returnUrl)));
                    }
                }

                return Redirect(string.Format(
                    "/utilisateur/erreur-association-compte-externe?provider={0}&returnUrl={1}&dm=false",
                    WebUtility.UrlEncode(loginProvider), WebUtility.UrlEncode(returnUrl)));
            }


            return Redirect(string.Format("/utilisateur/erreur-association-compte-externe?returnUrl={0}&dm=false",
                WebUtility.UrlEncode(returnUrl)));
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new {ReturnUrl = returnUrl});
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return UserManager.GetUserAsync(HttpContext.User);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await SignInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null) return Redirect(returnUrl);

            // Connecter cet utilisateur à ce fournisseur de connexion externe si l'utilisateur possède déjà une connexion
            var result =
                await SignInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, false);
            if (result.Succeeded) return Redirect(returnUrl);

            // Si l'utilisateur n'a pas de compte, invitez alors celui-ci à créer un compte
            var email = loginInfo.Principal.FindFirst(ClaimTypes.Email).Value;
            returnUrl = returnUrl.Replace("://", "----").Replace(":", "---");
            return Redirect(
                $"/utilisateur/confirmation-compte-externe?email={WebUtility.UrlEncode(email)}&provider={WebUtility.UrlEncode(loginInfo.LoginProvider)}&returnUrl={WebUtility.UrlEncode(returnUrl)}&dm=false");
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        //  [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation([FromBody] ExternalLoginConfirmationViewModel model)
        {
            if (User.Identity.IsAuthenticated) throw new Exception("Vous êtes déjà authentifié");
            // Obtenez des informations sur l’utilisateur auprès du fournisseur de connexions externe
            var commandResult = new CommandResult();

            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (ModelState.IsValid)
            {
                if (info == null)
                {
                    commandResult.ValidationResult.AddError("EXTERNAL_PROVIDER",
                        "Erreur lors de la récupération des informations du compte externe.");
                    return new JsonResult(commandResult);
                }

                var user = new ApplicationUser
                {
                    //Id= Guid.NewGuid(),
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedDate = new DateTime(),
                    FirstName = StringHelper.FirstLetterToUpper(model.FirstName),
                    LastName = StringHelper.FirstLetterToUpper(model.LastName)
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false);

                        // Pour plus d'informations sur l'activation de la confirmation du compte et la réinitialisation du mot de passe, consultez http://go.microsoft.com/fwlink/?LinkID=320771
                        //  Envoyer un message électronique contenant ce lien
                        await SaveUserCommand.SendConfirmEmailAsync(_emailService, UserManager,
                            new SendConfirmEmailModel
                            {
                                Email = user.Email,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Provider = model.Provider
                            }, user);

                        return new JsonResult(commandResult);
                    }
                }

                // TODO utiliser framework validation
                foreach (var error in result.Errors) commandResult.ValidationResult.AddError("Email", error.Code);
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var value = ModelState[key];
                    if (value.Errors.Count > 0)
                        commandResult.ValidationResult.AddError(key, value.Errors[0].ErrorMessage);
                }
            }

            return new JsonResult(commandResult);
        }


        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null) return Redirect("/utilisateur/confirmation-email-error?dm=false");
            IdentityResult result;
            try
            {
                var user = await UserManager.FindByIdAsync(userId);
                result = await UserManager.ConfirmEmailAsync(user, code);
            }
            catch (InvalidOperationException ioe)
            {
                // ConfirmEmailAsync throws when the userId is not found.
                return Redirect("/utilisateur/confirmation-email-error?dm=false");
            }

            if (result.Succeeded)
            {
                await SaveUserCommand.AttachRolesAsync(_userService, _siteUserService, userId);
                return Redirect("/utilisateur/confirmation-email?dm=false");
            }

            var commandResult = new CommandResult();
            // If we got this far, something failed.
            foreach (var error in result.Errors) commandResult.ValidationResult.AddError("Email", error.Code);

            return Redirect("/utilisateur/confirmation-email-error?dm=false");
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            var commandResult = new CommandResult();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    //Id=Guid.NewGuid(),
                    Email = model.Email,
                    UserName = model.Email,
                    CreatedDate = new DateTime(),
                    FirstName = StringHelper.FirstLetterToUpper(model.FirstName),
                    LastName = StringHelper.FirstLetterToUpper(model.LastName)
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    //  Envoyer un message électronique contenant ce lien
                    await SaveUserCommand.SendConfirmEmailAsync(_emailService, UserManager,
                        new SendConfirmEmailModel
                        {
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName
                        }, user);

                    return new JsonResult(commandResult);
                }

                // TODO utiliser framework validation
                foreach (var error in result.Errors) commandResult.ValidationResult.AddError("Email", error.Code);
            }
            else
            {
                foreach (var key in ModelState.Keys)
                {
                    var value = ModelState[key];
                    if (value.Errors.Count > 0)
                        commandResult.ValidationResult.AddError(key, value.Errors[0].ErrorMessage);
                }
            }

            // If we got this far, something failed, redisplay form
            return new JsonResult(commandResult);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginViewModel model)
        {
            var commandResult = new CommandResult();
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var value = ModelState[key];
                    if (value.Errors.Count > 0)
                        commandResult.ValidationResult.AddError(key, value.Errors[0].ErrorMessage);
                }

                return new JsonResult(commandResult);
            }

            var signedUser = await UserManager.FindByEmailAsync(model.Email);

            if (signedUser == null)
            {
                commandResult.ValidationResult.AddError("InvalidLogin", "InvalidLogin");
                return new JsonResult(commandResult);
            }
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result =
                await
                    SignInManager.PasswordSignInAsync(signedUser.UserName, model.Password, model.RememberMe,
                        false);
            switch (result.ToString())
            {
                case "Succeeded":
                    return new JsonResult(commandResult);
                case "Lockedout":
                    commandResult.ValidationResult.AddError("LockedOut", "LockedOut");
                    break;
                default:
                    commandResult.ValidationResult.AddError("InvalidLogin", "InvalidLogin");
                    break;
            }

            return new JsonResult(commandResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var commandResult = new CommandResult();
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var value = ModelState[key];
                    if (value.Errors.Count > 0)
                        commandResult.ValidationResult.AddError(key, value.Errors[0].ErrorMessage);
                }

                return new JsonResult(commandResult);
            }

            var signedUser = await UserManager.FindByEmailAsync(model.Email);
            var token = await UserManager.GeneratePasswordResetTokenAsync(signedUser);
            await SaveUserCommand.SendResetPasswordEmailAsync(_emailService, signedUser, token);
            return new JsonResult(commandResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var commandResult = new CommandResult();
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var value = ModelState[key];
                    if (value.Errors.Count > 0)
                        commandResult.ValidationResult.AddError(key, value.Errors[0].ErrorMessage);
                }

                return new JsonResult(commandResult);
            }

            var user = await UserManager.FindByIdAsync(model.UserId);
            var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Errors != null)
                foreach (var resultError in result.Errors)
                    commandResult.ValidationResult.AddError(resultError.Code);

            return new JsonResult(commandResult);
        }
    }
}