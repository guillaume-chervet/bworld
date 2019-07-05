using System;

namespace Demo.Domain.Contact
{
    public class ExtraParameters
    {
        public string BirthDepartment { get; set; } = "99";
        public DateTime BirthDate { get; set; }
        public string BirthDateValue => BirthDate.ToString("yyyy-MM-dd");
        public CountryIso BirthCountry { get; set; }
        public string BirthCountryValue => BirthCountry.ToString().ToUpper();
        public string BirthCity { get; set; }
    }
}