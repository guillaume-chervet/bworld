using System;
using System.Threading.Tasks;
using Demo.Business.Command.Administration.Models;
using Demo.Business.Command.Site.Seo;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Message.Models;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Administration
{
    public class GetAdministrationCommand : Command<UserInput<string>, CommandResult<AdministrationModel>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetAdministrationCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, Input.Data);

            //  int sizeBytes = await Data.ItemRepository.CountSiteSizeBytesAsync(Input);

           // var numberUnreadMessage = await _messageService.CountUnreadChatAsync(new BoxId() {Id = Input.Data, Type = TypeBox.Site},
           //     new BoxId() {Id = Input.UserId, Type = TypeBox.User});


            var administrationModel = new AdministrationModel();

         //   administrationModel.NumberUnreadMessage = numberUnreadMessage;
            /*administrationModel.MaxTotalSizeBytes = 1048576*24;
            administrationModel.TotalSizeBytes = sizeBytes;*/

            // var seo = await GetSeoCommand.GetSeoItemDataModelAsync(Data, Input.Data);
            //administrationModel.IdSeoWarning = seo.

            Result.Data = administrationModel;

            /*long giga = sizeBytes/8589934592;
            int mega = sizeBytes/1048576;
            int kilo = (sizeBytes - mega*1048576)/1024;*/
        }

        private static string FormatBytes(long bytes)
        {
            string[] suffix = {"B", "KB", "MB", "GB", "TB"};
            int i;
            double dblSByte = bytes;
            for (i = 0; i < suffix.Length && bytes >= 1024; i++, bytes /= 1024)
                dblSByte = bytes/1024.0;
            return string.Format("{0:0.##}{1}", dblSByte, suffix[i]);
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }
    }
}