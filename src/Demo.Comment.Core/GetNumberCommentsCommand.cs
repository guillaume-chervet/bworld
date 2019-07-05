using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data.Comment;
using Demo.User.Identity;

namespace Demo.Business.Command.Comment
{
    public class GetNumberCommentsCommand : Command<GetNumberCommentsInput, CommandResult<GetNumberCommentsResult>>
    {
        private readonly ICommentService _commentService;

        public GetNumberCommentsCommand(ICommentService commentService)
        {
            _commentService = commentService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {

            var result = new GetNumberCommentsResult();
            result.Comments = new Dictionary<string, int>();
            foreach (var moduleId in Input.ModuleIds)
            {
                var count = await _commentService.CountCommentsAsync(moduleId);
                result.Comments.Add(moduleId, count);  
            }

            Result.Data = result;
        }
    }
}