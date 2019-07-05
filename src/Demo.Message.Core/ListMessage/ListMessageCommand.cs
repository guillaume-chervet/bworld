using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command.Contact.Message.Models.ListMessage;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Message.Models;
using Demo.Log;
using Demo.User;
using Demo.User.Identity;
using Microsoft.Extensions.Logging;

namespace Demo.Business.Command.Contact.Message
{
    public class ListMessageCommand : Command<UserInput<ListMessageInput>, CommandResult<ListMessageResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly IMessageService _messageService;
        private readonly ISiteMap _sitemap;
        private readonly ILogger<ListMessageCommand> _logger;
        private readonly UserService _userService;

        public ListMessageCommand(ILogger<ListMessageCommand> logger,IDataFactory dataFactory, UserService userService, IMessageService messageService, ISiteMap sitemap)
        {
            _logger = logger;
            _userService = userService;
            _messageService = messageService;
            _sitemap = sitemap;
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

            var limit = 20;

            var getChatsFilter = new GetChatsFilter
            {
                Limit = limit
            };

            var filterDate = Input.Data.Filter.Date;
            if (filterDate.HasValue)
            {
                if (Input.Data.Filter.IsPrevious)
                {
                    getChatsFilter.DateGt = filterDate.Value.ToLocalTime();
                }
                else
                {
                    getChatsFilter.DateLt = filterDate.Value.ToLocalTime();
                }
            }
            else
            {
                getChatsFilter.DateLt = DateTime.Now;
            }

            var chats = await _messageService.GetChatsAsync(boxId, getChatsFilter);

            var chatsResult = new List<ChatItem>();
            foreach (var chat in chats)
            {
                try
                {
                    var chatItem = await MapChatItem(_logger, _sitemap,_userService, chat, Input.UserId);
                    chatsResult.Add(chatItem);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Exception deserialize message ChatItem json");
                }
            }

            var lastItem = chatsResult.OrderBy(c => c.LastMessageDate).FirstOrDefault();
            var firstItem = chatsResult.OrderByDescending(c => c.LastMessageDate).FirstOrDefault();

            var datePrevious = firstItem?.LastMessageDate;
            var dateNext = lastItem?.LastMessageDate;
            long nbNext = 0;
            long nbPrevious = 0;
            if (datePrevious.HasValue)
            {
                nbPrevious =
                    await _messageService.CountChatAsync(boxId, new CountChatFilters {DateGt = datePrevious.Value});
            }
            if (dateNext.HasValue)
            {
                nbNext = await _messageService.CountChatAsync(boxId, new CountChatFilters {DateLt = dateNext.Value});
            }

            Result.Data = new ListMessageResult();
            Result.Data.Chats = chatsResult;
            Result.Data.NumberNext = nbNext;
            Result.Data.DatePrevious = datePrevious;
            Result.Data.NumberPrevious = nbPrevious;
            Result.Data.DateNext = dateNext;
        }

        public static async Task<ChatItem> MapChatItem(ILogger _logger, ISiteMap siteMap, UserService userService,
            ChatDbModel chat, string userId)
        {
            var messages = chat.Messages;
            var chatItem = new ChatItem();
            chatItem.Id = chat.Id;
            chatItem.CreatedDate = chat.CreateDate;
            var lastMessageDate = chat.Messages.OrderByDescending(m => m.CreateDate).Select(m => m.CreateDate).FirstOrDefault();
            chatItem.LastMessageDate = lastMessageDate;

            chatItem.Title = chat.Title;
            chatItem.Messages = new List<MessageItem>();
            chatItem.Readed = chat.LastReads != null && chat.LastReads.Count(b => b.Id.Id == userId && b.Id.Type == TypeBox.User ) > 0;

            chatItem.To = new List<ContactItem>();
            foreach (var to in chat.To)
            {
                var contactTo = await GetContactItem(_logger, siteMap, userService, to.Id, chat);
                chatItem.To.Add(contactTo);
            }

            foreach (var message in messages)
            {
                var messageItem = new MessageItem();
                messageItem.CreatedDate = chat.CreateDate;
                messageItem.LastMessageDate = chat.UpdateDate;
                messageItem.Title = chat.Title;

                var fromBoxId = message.FromId;
                var contact = await GetContactItem(_logger, siteMap, userService, fromBoxId, chat);

                messageItem.From = contact;
               
                messageItem.Message = message.Message;
                messageItem.MessageType = message.MessageType;
                messageItem.CreatedDate = message.CreateDate;

                chatItem.Messages.Add(messageItem);
            }
            return chatItem;
        }

        private static async Task<ContactItem> GetContactItem(ILogger _logger ,ISiteMap siteMap, UserService userService,
            BoxId fromBoxId, ChatDbModel chatDbModel = null)
        {
            var contact = new ContactItem();
            if (!string.IsNullOrEmpty(fromBoxId?.Id))
            {
                if (fromBoxId.Type == TypeBox.User)
                {
                    var userDb = await userService.FindApplicationUserByIdAsync(fromBoxId.Id);
                    contact.FullName = userDb.FullName;
                    contact.Id = fromBoxId.Id;
                    contact.Type = TypeBox.User;
                }
                else if (fromBoxId.Type == TypeBox.Site)
                {
                    contact.FullName = await siteMap.GetSiteNameAsync(fromBoxId.Id);
                    contact.Id = fromBoxId.Id;
                    contact.Type = TypeBox.Site;
                }
                else if (fromBoxId.Type == TypeBox.UserNotAuthenticated)
                {
                    InitContactNotAUthenticated(_logger,chatDbModel, contact);
                }
            }
            else
            {
                InitContactNotAUthenticated(_logger,chatDbModel, contact);
            }
            return contact;
        }

        private static void InitContactNotAUthenticated(ILogger _logger ,ChatDbModel chatDbModel, ContactItem contact)
        {
            try
            {
                var message = SendMessageCommand.FindUserNotAUthenticatedInfo(chatDbModel);
                if (message != null)
                {
                    contact.FullName = message.FullName;
                    contact.Id = message.Email;
                    contact.Type = TypeBox.UserNotAuthenticated;
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception deserialize message ContactItem json");
            }
        }
        
    }
}