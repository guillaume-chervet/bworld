﻿using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.UserCore
{
    public class GetUserInfoCommand : Command<UserInput<string>, CommandResult<GetUserInfoResult>>
    {
        private readonly UserService _userService;

        public GetUserInfoCommand(UserService userService)
        {
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            if (!string.IsNullOrEmpty(Input.UserId))
            {
                var user = await _userService.FindApplicationUserByIdAsync(Input.UserId);
                if (user != null)
                {
                   
                    Result.Data = new GetUserInfoResult()
                    {
                        User = MapUser.Map( user)
                    };
                }
                else
                {
                    Result.ValidationResult.AddError("NOT_AUTHENTICATED");
                }
            }
            else
            {
                Result.ValidationResult.AddError("NOT_AUTHENTICATED");
            }

        }
    }
}