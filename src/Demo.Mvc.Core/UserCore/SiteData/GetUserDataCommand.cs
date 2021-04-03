using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.User.SiteData;

namespace Demo.Mvc.Core.UserCore.SiteData
{
    public class GetUserDataCommand : Command<UserInput<GetUserDataInput>, CommandResult<InputUserDataResult>>
    {
        private readonly UserDataService _userDataService;

        public GetUserDataCommand(UserDataService userDataService) 
        {
            _userDataService = userDataService;
        }
        
        protected override async Task ActionAsync()
        {
            var data = Input.Data;
            var datas = await _userDataService.GetAsync(data.SiteId, Input.UserId);
            Result.Data = new InputUserDataResult()
            {
                Datas = datas
            };
        }

    }
}