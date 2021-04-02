using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Common;
using Demo.Mvc.Core.Email;
using Demo.Mvc.Core.Renderer;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.Mvc.Core.UserCore.Site;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Mvc.Core.Sites.Core.Command.User.SaveSiteUser
{
    public class SaveSiteUserCommand : Command<UserInput<SaveSiteUserInput>, CommandResult<SaveSiteUserOutput>>
    {
        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;
        private readonly IEmailService _emailService;
        private readonly SiteUserService _siteUserService;
        private readonly IRouteManager _routeManager;

        public SaveSiteUserCommand(UserService userService, IDataFactory dataFactory, IEmailService emailService, SiteUserService siteUserService, IRouteManager routeManager)
        {
            _userService = userService;
            _dataFactory = dataFactory;
            _emailService = emailService;
            _siteUserService = siteUserService;
            _routeManager = routeManager;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var userDb = await _userService.FindApplicationUserByIdAsync(Input.UserId);

             var result = await SaveUserAsync(_siteUserService, _userService, userDb, Input.Data);

            if (result.SendMailAdmin != null)
            {
                await SendEmailAsync(_routeManager,_dataFactory, _emailService, result.SendMailAdmin);
            }

            Result.Data = new SaveSiteUserOutput()
            {
                SiteUserId = result.SiteUserId
            };
            if (!string.IsNullOrEmpty(result.Error))
            {
                Result.ValidationResult.AddError(result.Error);
            }

        }

        public static async Task<SaveUserResult> CreateNewSiteUserAsync(SiteUserService _siteUserService, UserService _userService, string siteId, string userId)
        {

            var userDbModel = await _userService.FindApplicationUserByIdAsync(userId);
            var saveUserInput = new SaveSiteUserInput
            {
                SiteId = siteId,
                Mail = userDbModel.Email,
                FirstName = userDbModel.FirstName,
                LastName = userDbModel.LastName,
                Birthdate = null,
                Roles = new List<SiteUserRole>() { SiteUserRole.Administrator }  
            };
            var result = await SaveUserAsync(_siteUserService, _userService, null, saveUserInput);
           
            return result;
        }

        public static async Task<SaveUserResult> SaveUserAsync(SiteUserService _siteUserService, UserService _userService, ApplicationUser creatorUserDb, SaveSiteUserInput saveSiteUser)
        {
            var result = new SaveUserResult();
            
            var siteId = saveSiteUser.SiteId;
            var mail = saveSiteUser.Mail.ToLower().Trim();
            SiteUserDbModel siteUserDbModel = null;

            var siteUserId = saveSiteUser.SiteUserId;
            if (String.IsNullOrEmpty(siteUserId))
            {
                // 1: Creation
                siteUserDbModel = await _siteUserService.FindByEmailAsync(siteId, mail);
                if (siteUserDbModel != null)
                {
                    result.Error = "user_email_already_added";
                    result.IsSuccess = false;
                    result.SiteUserId = siteUserDbModel.Id;
                    return result;
                }
                siteUserDbModel = new SiteUserDbModel();
                siteUserDbModel.SiteId = siteId;
            }
            else
            {
                // 1: Mise à jour
                siteUserDbModel = await _siteUserService.FindAsync(siteUserId);
                var siteUserDbModelCheck = await _siteUserService.FindByEmailAsync(siteId, mail);
                if (siteUserDbModelCheck!=null && siteUserDbModel.Id != siteUserDbModelCheck.Id)
                {
                    result.Error = "user_email_already_added";
                    result.IsSuccess = false;
                    result.SiteUserId = siteUserDbModel.Id;
                    return result;
                }
            }

            // On doit créer le userSite
            siteUserDbModel.Mail = mail;
            siteUserDbModel.FirstName = saveSiteUser.FirstName;
            siteUserDbModel.LastName = saveSiteUser.LastName;
            siteUserDbModel.Birthdate = saveSiteUser.Birthdate;
            siteUserDbModel.Tags = saveSiteUser.Tags;
            siteUserDbModel.Comments = saveSiteUser.Comments;
            siteUserDbModel.Civility = saveSiteUser.Civility;
            siteUserDbModel.IsEmailNotif = saveSiteUser.IsEmailNotif;
            siteUserDbModel.FlaggedRoles = saveSiteUser.Roles;

            var updatedUserDb = await _userService.FindApplicationUserByEmailAsync(mail);
            if (updatedUserDb != null)
            {
                siteUserDbModel.UserId = updatedUserDb.Id;
            }

            await _siteUserService.SaveAsync(siteUserDbModel);
            result.SiteUserId = siteUserDbModel.Id;

            var resultManageRoleAdministrator = await ManageRolesAsync(_userService, saveSiteUser, updatedUserDb, siteId, SiteUserRole.Administrator);
            switch (resultManageRoleAdministrator)
            {
                case "user_updated":
                    result.SendMailAdmin = new SendMailAdmin()
                    {
                        Mail = mail,
                        UserCreated = updatedUserDb,
                        SiteId = siteId,
                        UserCreator = creatorUserDb
                    };
                    break;
                case "no_user_found":
                    result.SendMailAdmin = new SendMailAdmin()
                    {
                        Mail = mail,
                        SiteId = siteId,
                        UserCreator = creatorUserDb
                    };
                    result.Error = "no_user_found";
                    break;
            }

            var resultManageRoleUserPrivate = await ManageRolesAsync(_userService, saveSiteUser, updatedUserDb, siteId+ "_private_user", SiteUserRole.PrivateUser);

            return result;
        }

        private static async Task<string> ManageRolesAsync(UserService _userService, SaveSiteUserInput saveSiteUser,
            ApplicationUser updatedUserDb, string role, SiteUserRole siteUserRole)
        {
            if (saveSiteUser.Roles != null && saveSiteUser.Roles.Contains(siteUserRole))
            {
                if (updatedUserDb != null)
                {
                    if (updatedUserDb.Roles.Count(c => c == role)<=0)
                    {
                        updatedUserDb.AddClaim(new Claim(role, ""));
                        await _userService.SaveAsync(updatedUserDb);
                    }
                    return "user_updated";
                }
                else
                {

                    return "no_user_found";
                }
            }
            else
            {
                var claim = updatedUserDb.Roles.FirstOrDefault(c => c == role);
                if (updatedUserDb != null && claim != null)
                {
                    updatedUserDb.Roles.Remove(claim);
                    await _userService.SaveAsync(updatedUserDb);
                }
            }
            return String.Empty;
        }

        private static async Task SendEmailAsync(IRouteManager routeManager ,IDataFactory _dataFactory, IEmailService _emailService,
            SendMailAdmin sendMailAdmin)
        {

            var siteInfo = await SiteMap.SiteUrlAsync(routeManager,_dataFactory, sendMailAdmin.SiteId);

            if (sendMailAdmin.UserCreated == null)
            {
                await SendMailNoUserAsync(siteInfo, _emailService    , sendMailAdmin.UserCreator, sendMailAdmin.Mail);
            }
            else
            {
                await SendEmailAdminAsync(siteInfo, _emailService, sendMailAdmin.UserCreated, sendMailAdmin.SiteId, sendMailAdmin.UserCreator, sendMailAdmin.Mail);
            }

        }

        private static async Task SendEmailAdminAsync(SiteMap.Site siteInfo, IEmailService _emailService, ApplicationUser modifiedUserDb, string siteId, ApplicationUser userDb, string mail)
        {      
                var model = new AdministrationAddUserFoundModel()
                {
                    AdderUserName = userDb.FullName,
                    UserName = modifiedUserDb.FullName,
                    SiteName = siteInfo.Name,
                    SiteUrl = siteInfo.Url,
                };

                var identityMessage = new MailMessage();
                identityMessage.Subject = string.Format("[{0}] {1} vous a donné les droits administrateur", model.SiteName,
                    model.AdderUserName);
                identityMessage.Body = new StringTemplateRenderer().Render(
                    ResourcesLoader.Load(String.Concat("Sites", "Core", "Command", "User", "SaveSiteUser","AdministrationAddUserfound.st")), model);
                identityMessage.Destination = modifiedUserDb.Email;
                await _emailService.SendAsync(identityMessage);
        }

        private static async Task<SiteMap.Site> SendMailNoUserAsync(SiteMap.Site siteInfo, IEmailService _emailService, ApplicationUser userDb, string mail)
        {
            var model = new AdministrationAddUserNotFoundModel()
            {
                AdderUserName = userDb.FullName,
                SiteName = siteInfo.Name,
                SiteUrl = siteInfo.Url,
            };
            var identityMessage = new MailMessage();
            identityMessage.Subject = string.Format("[{0}] {1} vous a donné les droits administrateur", model.SiteName,
                model.AdderUserName);
            identityMessage.Body = new StringTemplateRenderer().Render(
                ResourcesLoader.Load(String.Concat("Sites", "Core", "Command", "User", "SaveSiteUser","AdministrationAddUserNotfound.st")), model);
            identityMessage.Destination = mail;
            await _emailService.SendAsync(identityMessage);
           
            return siteInfo;
        }
    }
}
