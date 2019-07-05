using System.Threading.Tasks;
using Demo.Data.Comment.Models;

namespace Demo.Data.Comment
{
    public interface ICommentService
    {
        Task SaveCommentAsync(string siteId, string moduleId, CommentDbModel commentDbModel);
        Task<CommentsDbModel> GetCommentsAsync(string moduleId);
        Task<int> CountCommentsAsync(string moduleId);
        Task DeleteCommentAsync(string siteId, string moduleId, string commentId);
    }
}