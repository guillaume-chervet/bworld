using System.Collections.Generic;
using Demo.User.SiteData.Model;

namespace Demo.Business.Command.User.SiteData
{
    public class InputUserDataResult
    {

        public IList<UserDataDbModel> Datas { get; set; }
    }
}
