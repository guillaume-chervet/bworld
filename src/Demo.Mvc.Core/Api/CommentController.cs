/*using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.Command;
using Demo.Business.Command.Comment;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Angular.Api
{
   
    public class CommentController : ApiControllerBase
    {
        private readonly GetCommentsCommand _getCommentsCommand;
        private readonly AddCommentCommand _addCommentCommand;
        private readonly DeleteCommentCommand _deleteCommentCommand;
        private readonly GetNumberCommentsCommand _getNumberCommentsCommand; 

        public CommentController(BusinessFactory business, GetCommentsCommand getCommentsCommand, AddCommentCommand addCommentCommand, DeleteCommentCommand deleteCommentCommand, GetNumberCommentsCommand getNumberCommentsCommand)
            : base(business)
        {
            _getCommentsCommand = getCommentsCommand;
            _addCommentCommand = addCommentCommand;
            _deleteCommentCommand = deleteCommentCommand;
            _getNumberCommentsCommand = getNumberCommentsCommand;
        }

        [HttpGet]
        [Route("api/comment/get/{moduleId}")]
        public async Task<CommandResult> Get(string moduleId)
        {
            var result = await Business.InvokeAsync<GetCommentsCommand, GetCommentsInput, CommandResult<GetCommentsResult>>(_getCommentsCommand, new GetCommentsInput() {ModuleId = moduleId});
            return result;
        }

        [HttpPost]
        [Route("api/comment/count")]
        public async Task<CommandResult> Count([FromBody]GetNumberCommentsInput input)
        {
            var result = await Business.InvokeAsync<GetNumberCommentsCommand, GetNumberCommentsInput, CommandResult<GetNumberCommentsResult>>(_getNumberCommentsCommand, input);
            return result;
        }


        [Authorize]
       
        [HttpPost]
        [Route("api/comment/save")]
        public async Task< CommandResult> Save([FromBody]AddCommentInput updateFreeInput)
        {
            var userInput = new UserInput<AddCommentInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            var result = await 
                Business.InvokeAsync<AddCommentCommand, UserInput<AddCommentInput>, CommandResult<GetCommentsResult>>(_addCommentCommand,userInput);

            return result;
        }

        [Authorize]
       
        [HttpPost]
        [Route("api/comment/delete")]
        public async Task<CommandResult> Delete([FromBody]DeleteCommentInput updateFreeInput)
        {
            var userInput = new UserInput<DeleteCommentInput>
            {
                UserId = User.GetUserId(),
                Data = updateFreeInput
            };

            CommandResult<GetCommentsResult> result = await
                Business.InvokeAsync<DeleteCommentCommand, UserInput<DeleteCommentInput>, CommandResult<GetCommentsResult>>(_deleteCommentCommand, userInput);

            return result;
        }

    }
}*/

