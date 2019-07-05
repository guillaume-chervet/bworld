using System;
using Demo.User.Identity.Models;

namespace Demo.Business.Command.Comment
{
    public class CommentItem
    {
        public UserInfo UserInfo { get; set; }
        public string Comment { get; set; }
        public string Id { get; set; }
        public DateTime DateCreate { get; set; }
        public bool CanDelete { get; set; }
    }
}