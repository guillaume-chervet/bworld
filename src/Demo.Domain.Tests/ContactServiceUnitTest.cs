using System;
using System.Threading.Tasks;
using Demo.Domain.Contact;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Domain.Tests
{
    [TestClass]
    public class ContactServiceUnitTest
    {
      //  private readonly ContactService _contactService = new ContactService(new DomainConfig());

        [TestMethod]
        public async Task TestMethodGetInfo()
        {
        //    var version = await _contactService.InfoAsync("GC5741-GANDI");
        }

        [TestMethod]
        public async Task TestMethodCreate()
        {
            /*
              'given': 'First Name',
...   'family': 'Last Name',
...   'email': 'example@example.com',
...   'streetaddr': 'My Street Address',
...   'zip': '75011',
...   'city': 'Paris',
...   'country': 'FR',
...   'phone':'+33.123456789',
...   'type': 0,
...   'password': 'xxxxxxxx'}
            */

            var contactSpec = new ContactCreateFormDescription();
            contactSpec.Given = "Fist Name";
            contactSpec.Family = "Last Name";
            contactSpec.Email = "toto@gmail.com";
            contactSpec.StreetAddr = "My Street Address";
            contactSpec.Zip = "75011";
            contactSpec.City = "Paris";
            contactSpec.Country = CountryIso.Fr;
            contactSpec.Phone = "+33.123456789";
            contactSpec.Password = "xxxxxxxx";
            contactSpec.Type = ContactType.Person;
            contactSpec.ExtraParameters = new ExtraParameters
            {
                BirthDepartment = "22440",
                BirthCity = "Ploufragan",
                BirthCountry = CountryIso.Fr,
                BirthDate = (DateTime.Now).AddYears(-25)
            };

          //  var version = await _contactService.CreateAsync(contactSpec);
        }

        [TestMethod]
        public async Task TestMethodCanAssociateDomainAsync()
        {
            var input = new CanAssociateDomainInput();
            input.Handle = "FLN1107-GANDI";
            input.Input = new ContactCheckDomain();
            input.Input.Domain = "dqshdkqjhdkjqsdh.fr";

            //var version = await _contactService.CanAssociateDomainAsync(input);
        }
    }
}