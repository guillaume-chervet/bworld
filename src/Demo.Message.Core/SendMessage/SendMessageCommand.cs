using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.Contact.Message.Models;
using Demo.Business.Command.Contact.Message.Models.SendMessage;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Business.Renderers;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Message.Models;
using Demo.Email;
using Demo.Routing.Extentions;
using Demo.User.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Demo.Renderer;
using Microsoft.Extensions.Logging;
using Demo.Common;
using System.IO;

namespace Demo.Business.Command.Contact.Message
{
    public class SendMessageCommand : Command<UserInput<SendMessageInput>, CommandResult<SendMessageResult>>
    {
        public const string SiteNotAuthenticated = "SiteNotAuthenticated";
        private readonly IDataFactory _dataFactory;
        private readonly ISiteMap _siteMap;
        private readonly ILogger _logger;
        private readonly IEmailService _emailService;
        private readonly IMessageService _messageService;
        private readonly UserService _userService;

        public SendMessageCommand(UserService userService, IMessageService messageService, IEmailService emailService,
            IDataFactory dataFactory,ISiteMap siteMap,ILoggerFactory loggerFactory)
        {
            _userService = userService;
            _messageService = messageService;
            _emailService = emailService;
            _dataFactory = dataFactory;
            _siteMap = siteMap;
            _logger = loggerFactory.CreateLogger("SendMessageCommand");
        }

        protected override async Task ActionAsync()
        {
            var data = Input.Data;

            var messageJson = data.MessageJson;
            var messageType = data.Type;

            var message = GetMessage(messageType, messageJson);
            var createdDate = DateTime.Now;

            var messageDbModel = new MessageDbModel();
            messageDbModel.CreateDate = createdDate;
            messageDbModel.FromId = data.From;
            messageDbModel.Source = data.Source;
            messageDbModel.Message = data.MessageJson;
            messageDbModel.MessageType = data.Type;

            IList<To> toList = new List<To> {new To() {Id = data.To}, new To() {Id = data.From}};
            string chatId;
            ChatDbModel chatDbModel;
            Read read = null;

            // TODO control input
            if (!string.IsNullOrEmpty(Input.UserId))
            {
                read = new Read() {CreateDate = createdDate, Id = data.From};
            }

            if (string.IsNullOrEmpty(data.ChatId))
            {
                var reads = new List<Read>();
                if (read != null)
                {
                    reads.Add(read);
                }

                chatDbModel = new ChatDbModel();
                chatDbModel.CreateDate = createdDate;
                chatDbModel.UpdateDate = createdDate;
                chatDbModel.Title = ((MessageSiteAuthenticated) message).Title;
                chatDbModel.Reads = reads;
                chatDbModel.LastReads = reads;
                chatDbModel.To = toList;
                chatDbModel.Messages = new List<MessageDbModel> {messageDbModel};
                chatDbModel.NumberMessages = 1;

                await _messageService.SaveChatAsync(chatDbModel);
                chatId = chatDbModel.Id;
            }
            else
            {
                await _messageService.SaveMessageAsync(data.ChatId, messageDbModel, read);
                chatDbModel = await _messageService.GetChatAsync(data.ChatId);
                toList = chatDbModel.To;
                chatId = chatDbModel.Id;
            }

            Result.Data = new SendMessageResult()
            {
                Chat = await ListMessageCommand.MapChatItem(_logger, _siteMap, _userService, chatDbModel, Input.UserId)
            };

            ApplicationUser senderUserDb = null;
            if (!string.IsNullOrEmpty(data.From.Id) && data.From.Type == TypeBox.User)
            {
                senderUserDb = await _userService.FindApplicationUserByIdAsync(data.From.Id);
            }

            var siteId = toList.Where(t => t.Id.Type == TypeBox.Site).Select(t => t.Id.Id).FirstOrDefault();

            var site = await _siteMap.SiteUrlAsync(siteId);
            var siteName = site.Name;
            var siteUrl = site.Url;
            var isReply = data.Type == "Reply";

            foreach (var to in toList)
            {
                if (to.Id.Type == TypeBox.Site && !string.IsNullOrEmpty(to.Id.Id))
                {
                    var users = await _userService.UserByRoleAsync(siteId);
                    foreach (var userDb in users)
                    {
                        await
                            SendMessageReceiver(siteName, siteUrl, userDb, message, senderUserDb, messageType,
                                chatDbModel, true);
                    }
                }
                else if (to.Id.Type == TypeBox.User && !string.IsNullOrEmpty(to.Id.Id))
                {
                    var userDb = await _userService.FindApplicationUserByIdAsync(to.Id.Id);
                    await
                        SendMessageReceiver(siteName, siteUrl, userDb, message, senderUserDb, messageType, chatDbModel, false);
                } /*else if (to.Id.Type == TypeBox.UserNotAuthenticated)
                {
                    var messageUserNotAuthenticated = FindUserNotAUthenticatedInfo(chatDbModel);
                    await
                        SendMessageReceiver(siteName, siteUrl, null, messageUserNotAuthenticated, senderUserDb, messageType, chatDbModel, false);
                }*/
            }

            await SendMessageSender(messageType, message, siteName, siteUrl, isReply, senderUserDb, data, chatId, chatDbModel);
        }

        internal static MessageSiteNotAuthenticated FindUserNotAUthenticatedInfo(ChatDbModel chatDbModel){

            if (chatDbModel == null || chatDbModel.Messages == null)
            {
                return null;
            }

            foreach (var messageDbModel in chatDbModel.Messages)
            {
                var messageType = messageDbModel.MessageType;
                if (messageDbModel.MessageType == SiteNotAuthenticated)
                {
                    return (MessageSiteNotAuthenticated) GetMessage(messageType, messageDbModel.Message);
                }
            }

            return null;
        }

        private async Task SendMessageSender(string messageType, MessageReply message, string siteName, string siteUrl, bool isReply,
            ApplicationUser senderUserDb, SendMessageInput data, string chatId, ChatDbModel chatDbModel)
        {
            if (messageType == SiteNotAuthenticated)
            {
                var msg = (MessageSiteNotAuthenticated) message;
                var messageReceiverMailModel = new MessageReceiverMailModel();
                messageReceiverMailModel.SiteName = siteName;
                messageReceiverMailModel.SiteUrl = siteUrl;
                messageReceiverMailModel.UserName = msg.FullName;
                messageReceiverMailModel.Title = msg.Title;
                messageReceiverMailModel.Message = FormatMessageForEmail(message.Message);
                messageReceiverMailModel.IsReply = isReply;
                messageReceiverMailModel.Sender = new SenderModel()
                {
                    IsNotAuthenticated = true,
                    FullName = msg.FullName,
                    Email = msg.Email,
                    Phone = msg.Phone
                };

                await SendEmailSenderAsync(messageReceiverMailModel, msg.Email);
            }
            else if (senderUserDb != null)
            {
                var messageReceiverMailModel = new MessageReceiverMailModel();
                messageReceiverMailModel.SiteName = siteName;
                messageReceiverMailModel.SiteUrl = siteUrl;

                var messageSource = "/administration";
                if (data.Source == "User")
                {
                    messageSource = "/utilisateur";
                }
                var messageUrl = UrlHelper.Concat(siteUrl, messageSource + "/messages/message/" + chatId);

                messageReceiverMailModel.MessageUrl = messageUrl;
                messageReceiverMailModel.UserName = senderUserDb.FullName;
                messageReceiverMailModel.Message = FormatMessageForEmail(message.Message);
                messageReceiverMailModel.Title = chatDbModel.Title;
                messageReceiverMailModel.Sender = new SenderModel()
                {
                    IsNotAuthenticated = false,
                    FullName = senderUserDb.FullName
                };
                messageReceiverMailModel.IsReply = isReply;

                await SendEmailSenderAsync(messageReceiverMailModel, senderUserDb.Email);
            }
        }

        private async Task SendMessageReceiver(string siteName, string siteUrl, ApplicationUser userDb,
            MessageReply messageReply, ApplicationUser senderUserDb, string messageType, ChatDbModel chatDbModel, bool isAdmin)
        {
            if (senderUserDb == null || senderUserDb.Id != userDb.Id)
            {
                var messageReceiverMailModel = new MessageReceiverMailModel();
                messageReceiverMailModel.SiteName = siteName;
                messageReceiverMailModel.SiteUrl = siteUrl;
                messageReceiverMailModel.UserName = userDb.FullName;
                messageReceiverMailModel.Message = FormatMessageForEmail(messageReply.Message);
                messageReceiverMailModel.Title = chatDbModel.Title;

                var messageSource = "/administration";
                if (!isAdmin)
                {
                    messageSource = "/utilisateur";
                }
                var messageUrl = UrlHelper.Concat(siteUrl, messageSource + "/messages/message/" + chatDbModel.Id);
                messageReceiverMailModel.MessageUrl = messageUrl;

                if (messageType == SiteNotAuthenticated)
                {
                    var msg = (MessageSiteNotAuthenticated) messageReply;

                    messageReceiverMailModel.Sender = new SenderModel()
                    {
                        Email = msg.Email,
                        FullName = msg.FullName,
                        Phone = msg.Phone,
                        IsNotAuthenticated = true
                    };
                }
                else if (senderUserDb != null)
                {
                    messageReceiverMailModel.Sender = new SenderModel()
                    {
                        FullName = senderUserDb.FullName,
                        IsNotAuthenticated = false
                    };
                }

                await SendEmailAsync(messageReceiverMailModel, userDb.Email);
            }
        }

        private static string FormatMessageForEmail(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return message;
            }
            return message.Replace("\n", "<br />");
        }

        public static MessageReply GetMessage(string messageType, string messageJson)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            MessageReply message;
            if (messageType == SiteNotAuthenticated)
            {
                message = JsonConvert.DeserializeObject<MessageSiteNotAuthenticated>(messageJson, jsonSerializerSettings);
            }
            else if (messageType == "Reply")
            {
                message = JsonConvert.DeserializeObject<MessageReply>(messageJson, jsonSerializerSettings);
            }
            else
            {
                message = JsonConvert.DeserializeObject<MessageSiteAuthenticated>(messageJson, jsonSerializerSettings);
            }
            return message;
        }

        public async Task SendEmailSenderAsync(MessageReceiverMailModel messageSenderMailModel, string emailTo)
        {
            var identityMessage = new MailMessage();

            var subject = string.Format("[{0}] Message envoyé par {1} {2}", messageSenderMailModel.SiteName, messageSenderMailModel.Sender.FullName, messageSenderMailModel.Title);

            if (messageSenderMailModel.IsReply)
            {
                subject = string.Format("[{0}] Réponse envoyé par {1} {2}", messageSenderMailModel.SiteName, messageSenderMailModel.Sender.FullName, messageSenderMailModel.Title);
            }

            identityMessage.Subject = subject;
            identityMessage.Body = new StringTemplateRenderer().Render(
               ResourcesLoader.Load(Path.Combine("Renderers", "MessageSender.st")), messageSenderMailModel);
            identityMessage.Destination = emailTo;
            await _emailService.SendAsync(identityMessage);
        }

        public async Task SendEmailAsync(MessageReceiverMailModel messageReceiverMailModel, string emailTo)
        {
            var identityMessage = new MailMessage();

            var subject = $"[{messageReceiverMailModel.SiteName}] Message reçu de {messageReceiverMailModel.Sender.FullName} {messageReceiverMailModel.Title}";

            if (messageReceiverMailModel.IsReply)
            {
                subject = $"[{messageReceiverMailModel.SiteName}] Réponse reçu de {messageReceiverMailModel.Sender.FullName} {messageReceiverMailModel.Title}";
            }

            identityMessage.Subject = subject;
            identityMessage.Body = new StringTemplateRenderer().Render(
                ResourcesLoader.Load(Path.Combine("Renderers", "MessageReceiver.st")), messageReceiverMailModel);
            identityMessage.Destination = emailTo;
            await _emailService.SendAsync(identityMessage);
        }

       
    }
}