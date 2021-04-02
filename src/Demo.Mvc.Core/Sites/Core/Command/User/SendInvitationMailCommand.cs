using System;
using System.IO;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Common;
using Demo.Mvc.Core.Email;
using Demo.Mvc.Core.Renderer;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.Renderers;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Mvc.Core.Sites.Core.Command.User
{
    public class SendInvitationMailCommand : Command<UserInput<SendInvitationMailInput>, CommandResult>
    {

        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;
        private readonly IEmailService _emailService;
        private readonly SiteUserService _siteUserService;
        private readonly IRouteManager _routeManager;

        public SendInvitationMailCommand(UserService userService, IDataFactory dataFactory, IEmailService emailService, SiteUserService siteUserService, IRouteManager routeManager)
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

            var siteUserDb = await _siteUserService.FindAsync(Input.Data.SiteUserId);

            if (!string.IsNullOrEmpty(siteUserDb.UserId))
            {
                throw new ArgumentException("L'utilisateur est déjà inscrit");
            }

            var siteInfo = await SiteMap.SiteUrlAsync(_routeManager,_dataFactory, Input.Data.SiteId);

            var userDb = await _userService.FindApplicationUserByIdAsync(Input.UserId);
            await SendEmailAsync(siteInfo, _emailService, siteUserDb, userDb);
        }

        private static async Task SendEmailAsync(SiteMap.Site siteInfo, IEmailService _emailService, SiteUserDbModel friendUserDb,  ApplicationUser userDb)
        {
            var model = new MailInvitation()
            {
                UserNameSender = userDb.FullName,
                UserName = friendUserDb.FullName,
                SiteName = siteInfo.Name,
                SiteUrl = siteInfo.Url,
            };

            var identityMessage = new MailMessage();
            identityMessage.Subject = string.Format("[{0}] {1} vous invite à vous inscrire sur le site", model.SiteName,
                model.UserNameSender);
            identityMessage.Body = new StringTemplateRenderer().Render(ResourcesLoader.Load(Path.Combine("Sites", "Core", "Renderers", "MailInvitation.st"))
                , model);
            identityMessage.Destination = friendUserDb.Mail;
            await _emailService.SendAsync(identityMessage);
        }

    }
}
