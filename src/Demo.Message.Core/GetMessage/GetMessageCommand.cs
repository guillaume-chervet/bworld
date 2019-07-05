using System;
using System.Threading.Tasks;
using Demo.Business.Command.Contact.Message.Models;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Message.Models;
using Demo.User;
using Demo.User.Identity;
using Microsoft.Extensions.Logging;

namespace Demo.Business.Command.Contact.Message
{
    public class GetMessageCommand : Command<UserInput<GetMessageInput>, CommandResult<GetMessageResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly IMessageService _messageService;
        private readonly ISiteMap _siteMap;
        private readonly ILogger _logger;
        private readonly UserService _userService;

        public GetMessageCommand(IDataFactory dataFactory, UserService userService, IMessageService messageService, ISiteMap siteMap, ILoggerFactory loggerFactory)
        {
            _userService = userService;
            _messageService = messageService;
            _siteMap = siteMap;
            this._logger = loggerFactory.CreateLogger("GetMessageCommand");
            _dataFactory = dataFactory;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            var boxId = Input.Data.BoxId;

            if (boxId.Type == TypeBox.Site)
            {
                await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, boxId.Id);
            }
            else if (boxId.Type == TypeBox.User)
            {
                if (boxId.Id != Input.UserId)
                {
                    throw new NotAuthorizedException("You are not authorized to process this action.");
                }
            }

            await _messageService.SaveRead(Input.Data.ChatId, new Read() {CreateDate = DateTime.Now, Id = new BoxId() { Id = Input.UserId, Type = TypeBox.User } });

            var chat = await _messageService.GetChatAsync(Input.Data.ChatId);
            var chatItem = await ListMessageCommand.MapChatItem(_logger, _siteMap, _userService, chat, Input.UserId);

            Result.Data = new GetMessageResult
            {
                Chat = chatItem
            };
        }
    }
}