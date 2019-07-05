using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Comment;
using Demo.User.Identity;

namespace Demo.Business.Command.Comment
{
    public class DeleteCommentCommand : Command<UserInput<DeleteCommentInput>, CommandResult<GetCommentsResult>>
    {
        private readonly ICommentService _commentService;
        private readonly UserService _userService;

        public DeleteCommentCommand(UserService userService, ICommentService commentService)
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
            if (string.IsNullOrEmpty(Input.UserId))
            {
                throw new NotAuthentifiedException("You are not authorized to process this action.");
            }

            if (string.IsNullOrEmpty(Input.Data.ModuleId))
            {
                throw new ArgumentException("ModuleId should not be empty");
            }

            if (string.IsNullOrEmpty(Input.Data.CommentId))
            {
                throw new ArgumentException("CommentId should not be empty");
            }

            var user = await _userService.FindApplicationUserByIdAsync(Input.UserId);

            var commentDbModel = await _commentService.GetCommentsAsync(Input.Data.ModuleId);

            var comment = commentDbModel.Comments.First(p => p.Id == Input.Data.CommentId);

            if (comment.UserId != user.Id)
            {
                Result.ValidationResult.AddError("401", "You are not authorized to process this action.");
            }

            await _commentService.DeleteCommentAsync(Input.Data.SiteId, Input.Data.ModuleId, Input.Data.CommentId);

            var result =
                await
                    AddCommentCommand.GetCommentsResult(Input.Data.ModuleId, _commentService, _userService, Input.UserId);
            Result.Data = result;
        }
    }
}