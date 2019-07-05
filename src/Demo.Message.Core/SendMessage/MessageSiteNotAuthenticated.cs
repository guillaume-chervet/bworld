using Demo.Common;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Contact.Message.Models.SendMessage
{
    public class MessageSiteNotAuthenticated : MessageSiteAuthenticated
    {
        private string _firstName;
        private string _lastName;
        public string Email { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = StringHelper.FirstLetterToUpper(value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = StringHelper.FirstLetterToUpper(value); }
        }

        public string Phone { get; set; }

        public string FullName
        {
            get { return string.Concat(FirstName, " ", LastName); }
        }
    }
}