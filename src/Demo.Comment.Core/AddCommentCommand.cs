using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.Contact.Message;
using Demo.Business.Renderers;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Comment;
using Demo.Data.Comment.Models;
using Demo.Email;
using Demo.Renderer;
using Demo.Routing;
using Demo.Routing.Extentions;
using Demo.Routing.Interfaces;
using Demo.Site.Helper;
using Demo.User.Identity;

namespace Demo.Business.Command.Comment
{
    public class AddCommentCommand : Command<UserInput<AddCommentInput>, CommandResult<GetCommentsResult>>
    {
        private readonly ICommentService _commentService;
        private readonly IDataFactory _dataFactory;
        private readonly IEmailService _emailService;
        private readonly IRouteManager _routeManager;
        private readonly UserService _userService;

        public AddCommentCommand(IDataFactory dataFactory, UserService userService, ICommentService commentService,
            IEmailService emailService, IRouteManager routeManager)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _commentService = commentService;
            _emailService = emailService;
            _routeManager = routeManager;
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

            if (string.IsNullOrEmpty(Input.Data.Comment))
            {
                throw new ArgumentException("Comment should not be empty");
            }


            var comment = new CommentDbModel();
            comment.UserId = Input.UserId;
            comment.Comment = Input.Data.Comment;
            comment.DateCreate = DateTime.Now;

            var moduleId = Input.Data.ModuleId;
            var siteId = Input.Data.SiteId;
            await _commentService.SaveCommentAsync(siteId, moduleId, comment);

            var comments = await _commentService.GetCommentsAsync(moduleId);

            await SendMail(_routeManager,siteId, comments);

            var result = await GetCommentsResult(_userService, comments, Input.UserId);

            Result.Data = result;
        }

        public static async Task<GetCommentsResult> GetCommentsResult(string moduleId, ICommentService _commentService,
            UserService _userService, string userId)
        {
            var comments = await _commentService.GetCommentsAsync(moduleId);

            return await GetCommentsResult(_userService, comments, userId);
        }

        private static async Task<GetCommentsResult> GetCommentsResult(UserService _userService,
            CommentsDbModel comments, string userId)
        {
            var result = new GetCommentsResult();
            result.Comments = new List<CommentItem>();
            if (comments != null)
            {
                var list = comments.Comments.Where(c => !c.IsDeleted);
                foreach (var comment1 in list)
                {
                    var commentItem = new CommentItem();
                    commentItem.Comment = comment1.Comment;
                    commentItem.Id = comment1.Id;
                    commentItem.DateCreate = comment1.DateCreate;
                    commentItem.UserInfo = await _userService.GetUserInfoAsync(comment1.UserId);
                    commentItem.CanDelete = comment1.UserId == userId;
                    result.Comments.Add(commentItem);
                }
            }
            return result;
        }

        private async Task SendMail(IRouteManager routeManager ,string siteId, CommentsDbModel commentsDbModel)
        {
            /* (new BusinessModuleFactory()).GetModule("NewsItem").GetInfoAsync(new GetModuleInput()
            {
                
            });*/

            var site = await SiteMap.SiteUrlAsync( routeManager,_dataFactory, siteId);

            var userIds = commentsDbModel.Comments
                .GroupBy(p => p.UserId)
                .Select(g => g.First().UserId)
                .ToList();

            var administrators = await _userService.UserByRoleAsync(siteId);

            foreach (var administrator in administrators)
            {
                if (!userIds.Contains(administrator.Id))
                {
                    userIds.Add(administrator.Id);
                }
            }

            foreach (var userId in userIds)
            {
                var user = await _userService.FindApplicationUserByIdAsync(userId);
                if (user.IsUserNotifyComment && user.Id != Input.UserId)
                {
                    var model = new CommentAddedMailModel
                    {
                        SiteName = site.Name,
                        SiteUrl = site.Url,
                        UserName = user.FullName,
                        ArticleTitle = Input.Data.ArticleTitle,
                        ArticleUrl = UrlHelper.Concat(site.Url, Input.Data.UrlPath)
                    };

                    var identityMessage = new MailMessage();
                    identityMessage.Subject = string.Format("[{0}] Commentaire envoyé sur le site", model.SiteName);
                  //TODO  identityMessage.Body = new StringTemplateRenderer().Render(
                        //Encoding.UTF8.GetString(TemplateResource.CommentAdded), model);
                    identityMessage.Destination = user.Email;
                    await _emailService.SendAsync(identityMessage);
                }
            }
        }
    }
}