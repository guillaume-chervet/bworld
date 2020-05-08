using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Email;
using Demo.Mvc.Core.Renderer;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Renderers;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;
using Demo.User.Site;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Core.Command.Notifications
{
    public class SendNotificationCommand : Command<UserInput<SendNotificationInput>, CommandResult>
    {

        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;
        private readonly IEmailService _emailService;
        private readonly SiteUserService _siteUserService;
        private readonly IRouteManager _routeManager;


        public SendNotificationCommand(UserService userService, IDataFactory dataFactory, IEmailService emailService, SiteUserService siteUserService, IRouteManager routeManager)
        {
            _userService = userService;
            _dataFactory = dataFactory;
            _emailService = emailService;
            _siteUserService = siteUserService;
            _routeManager = routeManager;
        }

        protected override async Task ActionAsync()
        {
            var _siteId = Input.Data.SiteId;
            
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, _siteId);

           // var notificationDb =  await GetAsync<NotificationBusinessModel>(_siteId, _siteId, NotificationBusinessModule.ModuleName, "Notification");

            var notificationItemDb = await SaveFreeCommand.SaveItemDataModelAsync<NotificationItemBusinessModel>(_dataFactory, Input.Data.Data, Input.UserId, NotificationItemBusinessModule.ModuleName);

            var elements =
            await SaveFreeCommand.GetElementsAsync(_dataFactory, notificationItemDb, Input.Data.Data.Elements);
            // On enregistre l'element
            var freeBusinessModel = (NotificationItemBusinessModel) notificationItemDb.Data;
            freeBusinessModel.Elements = elements;

            await _dataFactory.SaveChangeAsync();

            var siteInfo = await SiteMap.SiteUrlAsync(_routeManager,_dataFactory, _siteId);
            var userDb = await _userService.FindApplicationUserByIdAsync(Input.UserId);
            foreach (var siteUserId in Input.Data.SiteUserIds)
            {
                var siteUserDb = await _siteUserService.FindAsync(siteUserId);
                await SendEmailAsync(siteInfo, _emailService, siteUserDb, userDb, elements, _siteId);
            }

        }

        private static async Task SendEmailAsync(SiteMap.Site siteInfo, IEmailService _emailService, SiteUserDbModel friendUserDb,  ApplicationUser userDb, IList<Element> elements, string siteId)
        {
            var model = new MailInvitation()
            {
                UserNameSender = userDb.FullName,
                UserName = friendUserDb.FullName,
                SiteName = siteInfo.Name,
                SiteUrl = siteInfo.Url,
            };

            var title =  FreeBusinessModule.GetTitle(elements);

            var identityMessage = new MailMessage();
            identityMessage.Subject = new StringTemplateRenderer().Render(title, model);

            var bodyTemplate =  new StringBuilder();
            foreach (var element in elements)
            {
                bodyTemplate.Append("<div>");
                if (element.Type == "p")
                {
                    bodyTemplate.Append(element.Data);
                }
                else if (element.Type == "hr")
                {
                    bodyTemplate.Append("<hr />");
                }
                else if (SaveFreeCommand.IsFileElementType(element.Type))
                {
                    var files = JsonConvert.DeserializeObject<List<DataFileInput>>(element.Data);
                    foreach (var dataFile in files)
                    {
                        var uri =
                            $@"{siteInfo.Url}/api/file/get/{siteId}/{dataFile.Id}/{"ImageThumb"}/{UrlHelper
                                .NormalizeTextForUrl(dataFile.Name)}";

                        bodyTemplate.Append("<div><img src=\""+ uri + "\" /></div>");
                    }
                }
                bodyTemplate.Append("</div>");
            }

            identityMessage.Body = new StringTemplateRenderer().Render(bodyTemplate.ToString(), model);
            identityMessage.Destination = friendUserDb.Mail;
            await _emailService.SendAsync(identityMessage);
        }

    }
}
