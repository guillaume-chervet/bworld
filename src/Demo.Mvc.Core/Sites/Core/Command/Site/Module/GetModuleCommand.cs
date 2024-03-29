﻿using System;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Module
{
    public class GetModuleCommand : Command<UserInput<GetModuleInput>, CommandResult<Item>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetModuleCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);
            
            if (string.IsNullOrEmpty(Input.Data.ModuleId))
            {
                throw new ArgumentException("L'id du module est null ou vide");
            }

            var item =
                DataFactoryMongo.MapItemDataModelToItem(
                    await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId));

            Result.Data = item;
        }
    }
}