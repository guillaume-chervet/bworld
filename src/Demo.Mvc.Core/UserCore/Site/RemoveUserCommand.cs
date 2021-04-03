﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Email;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.UserCore.Site
{
    public class RemoveUserCommand : Command<UserInput<RemoveUserInput>, CommandResult>
    {
        private readonly UserService _userService;
        private readonly IDataFactory _dataFactory;
        private readonly IEmailService _emailService;

        public RemoveUserCommand(UserService userService, IDataFactory dataFactory, IEmailService emailService)
        {
            _userService = userService;
            _dataFactory = dataFactory;
            _emailService = emailService;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            if (Input.UserId == Input.Data.UserId)
            {
                throw new ArgumentException("User cannot remove is own role");
            }

            var modifiedUserDb = await _userService.FindApplicationUserByIdAsync(Input.Data.UserId);

            if (modifiedUserDb != null)
            {
                var claim = modifiedUserDb.Roles.FirstOrDefault(c => c == siteId);
                if (claim!=null)
                {
                    modifiedUserDb.Roles.Remove(claim);
                    await _userService.SaveAsync(modifiedUserDb);
                }
                else
                {
                    throw new ArgumentException("User do not have this role");
                }
            }
            else
            {
                throw new ArgumentException("User does no exist"); 
            }
        }
    }
}
