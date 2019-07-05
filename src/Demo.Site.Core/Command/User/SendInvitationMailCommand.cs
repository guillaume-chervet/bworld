
using Demo.Business.Renderers;
using Demo.Common.Command;
using Demo.Data;
using Demo.Email;
using Demo.Renderer;
using Demo.User.Identity;
using Demo.User.Site;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Demo.Common;
using Demo.Routing;
using Demo.Routing.Interfaces;
using Demo.User;
using Demo.Site.Helper;

namespace Demo.Business.Command.Administration.User
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

        protected override void Action()
        {
            throw new NotImplementedException();
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
            identityMessage.Body = new StringTemplateRenderer().Render(ResourcesLoader.Load(Path.Combine("Renderers", "MailInvitation.st"))
                , model);
            identityMessage.Destination = friendUserDb.Mail;
            await _emailService.SendAsync(identityMessage);
        }

    }
}
