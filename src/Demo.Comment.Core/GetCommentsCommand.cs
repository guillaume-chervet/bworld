using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Comment;
using Demo.User.Identity;

namespace Demo.Business.Command.Comment
{
    public class GetCommentsCommand : Command<GetCommentsInput, CommandResult<GetCommentsResult>>
    {
        private readonly ICommentService _commentService;
        private readonly UserService _userService;

        public GetCommentsCommand(UserService userService, ICommentService commentService)
        {
            _userService = userService;
            _commentService = commentService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            Result.Data =
                await AddCommentCommand.GetCommentsResult(Input.ModuleId, _commentService, _userService, string.Empty);
        }
    }
}