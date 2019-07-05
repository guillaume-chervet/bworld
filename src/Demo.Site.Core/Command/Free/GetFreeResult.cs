using System;
using System.Collections.Generic;
using Demo.Data.Model;
using Demo.User.Identity.Models;

namespace Demo.Business.Command.Free.Models
{
    public class GetFreeResult
    {
        public IList<Element> Elements { get; set; }
        public IList<string> Tags { get; set; }
        public ItemState State { get; set; }
        public string Icon { get; set; }
        public UserInfo UserInfo { get; set; }
        public UserInfo LastUpdateUserInfo { get; set; }

        public bool IsDisplayAuthor { get; set; }
        public bool IsDisplaySocial { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}