using System;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.User.SiteData;
using Demo.User.SiteData.Model;

namespace Demo.Business.Command.User.SiteData
{
    public class SaveUserDataCommand : Command<UserInput<SaveUserDataInput>, CommandResult<string>>
    {
        private readonly UserDataService _userDataService;

        public SaveUserDataCommand(UserDataService userDataService)
        {
            _userDataService = userDataService;
        }
        
        protected override async Task ActionAsync()
        {
            var data = Input.Data;
            // TODO controller les cookieSessionId
            
            //TODO valider les données
            
            Result.Data = await _userDataService.SaveAsync(new UserDataDbModel()
            {
                Id = data.Id,
                SiteId = data.SiteId,
                CookieSessionId = data.CookieSessionId,
                UserId = Input.UserId,
                ElementId = data.ElementId,
                BeginTicks = data.BeginTicks,
                EndTicks = data.EndTicks,
                Type = data.Type,
                ModuleId = data.ModuleId,
                Json = data.Json
            });
        }
    }
}