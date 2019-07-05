namespace Demo.Domain.Contact
{
    public class ContactCreateFormDescription
    {
        public string Given { get; set; }
        public string Family { get; set; }
        public string Email { get; set; }
        public string StreetAddr { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public CountryIso Country { get; set; }
        public string CountryValue => Country.ToString().ToUpper();
        public string Phone { get; set; }
        public string Password { get; set; }
        public ContactType Type { get; set; }
        public int TypeValue => (int) Type;
        public string SecurityQuestionNum { get; set; }
        public string SecurityQuestionAnswer { get; set; }
        public string Orgname { get; set; }
        public string Siren { get; set; }
        public bool? ThirdPartResell { get; set; } = false;
        public bool? DataObfuscated { get; set; } = false;
        public bool? MailObfuscated { get; set; } = false;
        public bool? Newsletter { get; set; } = false;
        public bool? AcceptContract { get; set; } = false;
        public string BrandNumber { get; set; }
        public string VatNumber { get; set; }
        public ExtraParameters ExtraParameters { get; set; }
    }
}