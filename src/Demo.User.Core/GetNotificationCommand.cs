using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data.Message;
using Demo.Data.Message.Models;

namespace Demo.Business.Command.User
{
    public class GetNotificationCommand : Command<UserInput<string>, CommandResult<GetNotificationResult>>
    {
        private readonly IMessageService _messageService;

        public GetNotificationCommand(IMessageService messageService)
        {
            _messageService = messageService;
        }

        protected override async Task ActionAsync()
        {
            var userBoxId = new BoxId() { Id = Input.UserId, Type = TypeBox.User };
            var siteBoxId = new BoxId() { Id = Input.Data, Type = TypeBox.Site };
            var numberUnreadMessage = await _messageService.CountUnreadChatAsync(userBoxId, userBoxId);
            var numberUnreadSiteMessage = await _messageService.CountUnreadChatAsync(siteBoxId, userBoxId);

            Result.Data =new GetNotificationResult()
            {
                NumberSiteUnreadMessage = numberUnreadSiteMessage,
                NumberUnreadMessage = numberUnreadMessage
            };

        }
    }
}
