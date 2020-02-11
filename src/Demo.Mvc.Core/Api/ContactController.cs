using System.Linq;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command;
using Demo.Business.Command.Contact.Message;
using Demo.Business.Command.Contact.Message.Models;
using Demo.Business.Command.Contact.Message.Models.ListMessage;
using Demo.Common.Command;
using Demo.Data.Message.Models;
using Demo.Message.Core.ListMessage;
using Demo.Mvc.Core.Api.Extentions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Api
{
    public class ContactController : ApiControllerBase
    {

        public ContactController(BusinessFactory business)
            : base(business)
        {
        }


        [HttpPost]
        [Route("api/contact/message")]
        public async Task<CommandResult<SendMessageResult>> Save([FromServices] SendMessageCommand sendMessageCommand,
            [FromBody] SendMessageInput input)
        {
            var userInput = new UserInput<SendMessageInput>
            {
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty,
                Data = input
            };

            var result = await
                Business.InvokeAsync<SendMessageCommand, UserInput<SendMessageInput>, CommandResult<SendMessageResult>>(
                    sendMessageCommand, userInput);

            return result;
        }

        [HttpGet]
        [Route("api/contact/messages/{typeBox}/{id}")]
        public async Task<CommandResult<ListMessageResult>> Messages(
            [FromServices] ListMessageCommand listMessageCommand, TypeBox typeBox, string id)
        {
            var queryString = Request.Query.ToDictionary(q => q.Key, q => q.Value);
            MessagesFilter messagesFilter;
            if (queryString.ContainsKey("f"))
            {
                var f = queryString["f"];
                messagesFilter = JsonConvert.DeserializeObject<MessagesFilter>(f);
            }
            else
            {
                messagesFilter = new MessagesFilter();
            }

            var sendMessageInput =
                new ListMessageInput {BoxId = new BoxId {Id = id, Type = typeBox}, Filter = messagesFilter};
            var userInput = new UserInput<ListMessageInput>
            {
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty,
                Data = sendMessageInput
            };

            var result =
                await Business
                    .InvokeAsync<ListMessageCommand, UserInput<ListMessageInput>, CommandResult<ListMessageResult>>(
                        listMessageCommand, userInput);

            return result;
        }


        [HttpGet]
        [Route("api/contact/message/{typeBox}/{boxId}/{chatId}")]
        public async Task<CommandResult<GetMessageResult>> Message([FromServices]GetMessageCommand getMessageCommand, TypeBox typeBox, string boxId, string chatId)
        {
            var sendMessageInput = new GetMessageInput
            {
                ChatId = chatId,
                BoxId = new BoxId {Id = boxId, Type = typeBox}
            };
            var userInput = new UserInput<GetMessageInput>
            {
                UserId = User.Identity.IsAuthenticated ? User.GetUserId() : string.Empty,
                Data = sendMessageInput
            };

            var result =
                await Business
                    .InvokeAsync<GetMessageCommand, UserInput<GetMessageInput>, CommandResult<GetMessageResult>>(
                        getMessageCommand, userInput);

            return result;
        }
    }
}