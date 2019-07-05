using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Demo.Domain.Contact
{
    public class ContactService : ServiceBase
    {
        public ContactService(IOptions<DomainConfig> options) : base(options)
        {
        }
        
        public async Task<ContactReturn> InfoAsync(string handle)
        {
            var requestTemplate = @"
               <methodCall>
                <methodName>contact.info</methodName>
                <params>
                  <param>
                     <value><string>§model.ApiKey§</string></value>
                     </param>
                     <param>§model.Data§</param>
                  </params>
               </methodCall>";

            var methodResponse = await MethodResponse(requestTemplate, handle);

            var result = ParseContactReturn(methodResponse);
            /*
            {
    "?xml": {
        "@version": "1.0"
    },
    "methodResponse": {
        "params": {
            "param": {
                "value": {
                    "struct": {
                        "member": [
                            {
                                "name": "family",
                                "value": {
                                    "string": "CHERVET"
                                }
                            },
                            {
                                "name": "community",
                                "value": {
                                    "boolean": "0"
                                }
                            },
                            {
                                "name": "vat_number",
                                "value": {
                                    "nil": null
                                }
                            },
                            {
                                "name": "newsletter",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "id",
                                "value": {
                                    "int": "83562"
                                }
                            },
                            {
                                "name": "is_corporate",
                                "value": {
                                    "boolean": "0"
                                }
                            },
                            {
                                "name": "city",
                                "value": {
                                    "string": "La Madeleine"
                                }
                            },
                            {
                                "name": "given",
                                "value": {
                                    "string": "Guillaume"
                                }
                            },
                            {
                                "name": "zip",
                                "value": {
                                    "string": "59110"
                                }
                            },
                            {
                                "name": "extra_parameters",
                                "value": {
                                    "struct": {
                                        "member": [
                                            {
                                                "name": "birth_date",
                                                "value": {
                                                    "string": ""
                                                }
                                            },
                                            {
                                                "name": "birth_department",
                                                "value": {
                                                    "string": ""
                                                }
                                            },
                                            {
                                                "name": "birth_city",
                                                "value": {
                                                    "string": ""
                                                }
                                            },
                                            {
                                                "name": "birth_country",
                                                "value": {
                                                    "string": ""
                                                }
                                            }
                                        ]
                                    }
                                }
                            },
                            {
                                "name": "state",
                                "value": {
                                    "nil": null
                                }
                            },
                            {
                                "name": "security_question_answer",
                                "value": {
                                    "string": ""
                                }
                            },
                            {
                                "name": "type",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "email",
                                "value": {
                                    "string": "guillaume.chervet@gmail.com"
                                }
                            },
                            {
                                "name": "fax",
                                "value": {
                                    "nil": null
                                }
                            },
                            {
                                "name": "handle",
                                "value": {
                                    "string": "GC17-GANDI"
                                }
                            },
                            {
                                "name": "third_part_resell",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "data_obfuscated",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "phone",
                                "value": {
                                    "string": "+33.676975613"
                                }
                            },
                            {
                                "name": "lang",
                                "value": {
                                    "string": "en"
                                }
                            },
                            {
                                "name": "shippingaddress",
                                "value": {
                                    "struct": ""
                                }
                            },
                            {
                                "name": "streetaddr",
                                "value": {
                                    "string": "App 20, 1 rue de Flandre"
                                }
                            },
                            {
                                "name": "bu",
                                "value": {
                                    "struct": {
                                        "member": [
                                            {
                                                "name": "forbidden_tlds",
                                                "value": {
                                                    "array": {
                                                        "data": ""
                                                    }
                                                }
                                            },
                                            {
                                                "name": "id",
                                                "value": {
                                                    "int": "1"
                                                }
                                            },
                                            {
                                                "name": "name",
                                                "value": {
                                                    "string": "Gandi SAS"
                                                }
                                            }
                                        ]
                                    }
                                }
                            },
                            {
                                "name": "mobile",
                                "value": {
                                    "nil": null
                                }
                            },
                            {
                                "name": "country",
                                "value": {
                                    "string": "FR"
                                }
                            },
                            {
                                "name": "mail_obfuscated",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "reachability",
                                "value": {
                                    "string": "none"
                                }
                            },
                            {
                                "name": "security_question_num",
                                "value": {
                                    "int": "0"
                                }
                            },
                            {
                                "name": "validation",
                                "value": {
                                    "string": "none"
                                }
                            }
                        ]
                    }
                }
            }
        }
    }
}*/


            return result;
        }

        private ContactReturn ParseContactReturn(dynamic methodResponse)
        {
            var result = new ContactReturn();
            var data = methodResponse.methodResponse.@params.param;
            {
                var member = data.value.@struct.member;
                if (member is JArray)
                {
                    foreach (var m in member)
                    {
                        ParseContactElement(m, result);
                    }
                }
                else
                {
                    ParseContactElement(member, result);
                }
            }
            return result;
        }

        private void ParseContactElement(dynamic data, ContactReturn result)
        {
            string name = data.name;
            dynamic value = data.value;
            switch (name)
            {
                case "handle":
                    result.Handle = value.@string;
                    break;
                case "id":
                    result.Id = value.@int;
                    break;
                case "given":
                    result.Given = value.@string;
                    break;
                case "family":
                    result.Family = value.@string;
                    break;
                case "email":
                    result.Email = value.@string;
                    break;
                case "streetaddr":
                    result.Streetaddr = value.@string;
                    break;
                case "zip":
                    result.Zip = value.@string;
                    break;
                case "city":
                    result.City = value.@string;
                    break;
                case "country":
                    result.Country = value.@string;
                    break;
                case "phone":
                    result.Phone = value.@string;
                    break;
                case "type":
                    result.Type = value.@int;
                    break;
                default:
                    break;
            }
        }

        public async Task<ContactReturn> UpdateAsync(ContactCreateFormDescription contactCreateFormDescription)
        {
            return null;
        }

        public async Task<ContactReturn> CreateAsync(ContactCreateFormDescription contactCreateFormDescription)
        {
            /*
            0	person
1	company
2	association
3	public body
4	reseller

            contact_spec = {
...   'given': 'First Name',
...   'family': 'Last Name',
...   'email': 'example@example.com',
...   'streetaddr': 'My Street Address',
...   'zip': '75011',
...   'city': 'Paris',
...   'country': 'FR',
...   'phone':'+33.123456789',
...   'type': 0,
...   'password': 'xxxxxxxx'}
>>> contact = api.contact.create(apikey, contact_spec)
>>> contact['handle']
Note type of account takes the following values 0 for a private customer 1 for a company 2 for an association 3 for a public body
*/

            var requestTemplate = @"
   <methodCall>
    <methodName>contact.create</methodName>
    <params>
      <param>
         <value><string>§model.ApiKey§</string></value>
         </param>
<param>
   <struct>
       <member><name>given</name><value><string>§model.Data.Given§</string></value></member>
       <member><name>family</name><value><string>§model.Data.Family§</string></value></member>
       <member><name>email</name><value><string>§model.Data.Email§</string></value></member>
       <member><name>streetaddr</name><value><string>§model.Data.StreetAddr§</string></value></member>
       <member><name>zip</name><value><string>§model.Data.Zip§</string></value></member>
       <member><name>city</name><value><string>§model.Data.City§</string></value></member>
       <member><name>country</name><value><string>§model.Data.CountryValue§</string></value></member>
§if(model.Data.Phone)§
<member><name>phone</name><value><string>§model.Data.Phone§</string></value></member>
§endif§
<member><name>type</name><value><string>§model.Data.TypeValue§</string></value></member>
       <member><name>password</name><value><string>§model.Data.Password§</string></value></member>
§if(model.Data.SecurityQuestionNum)§
<member><name>security_question_num</name><value><string>§model.Data.SecurityQuestionNum§</string></value></member>
§endif§
§if(model.Data.SecurityQuestionAnswer)§      
<member><name>security_question_answer</name><value><string>§model.Data.SecurityQuestionAnswer§</string></value></member>
§endif§
§if(model.Data.Orgname)§      
<member><name>orgname</name><value><string>§model.Data.Orgname§</string></value></member>
§endif§
§if(model.Data.BrandNumber)§      
<member><name>brand_number</name><value><string>§model.Data.BrandNumber§</string></value></member>
§endif§
§if(model.Data.VatNumber)§      
<member><name>vat_number</name><value><string>§model.Data.VatNumber§</string></value></member>
§endif§
§if(model.Data.Siren)§      
<member><name>siren</name><value><string>§model.Data.Siren§</string></value></member>
§endif§
§if(model.Data.ThirdPartResell.HasValue)§      
<member><name>third_part_resell</name><value><string>§model.Data.ThirdPartResell.Value§</string></value></member>
§endif§
§if(model.Data.DataObfuscated.HasValue)§      
<member><name>data_obfuscated</name><value><string>§model.Data.DataObfuscated.Value§</string></value></member>
§endif§
§if(model.Data.MailObfuscated.HasValue)§      
<member><name>mail_obfuscated</name><value><string>§model.Data.MailObfuscated.Value§</string></value></member>
§endif§
§if(model.Data.Newsletter.HasValue)§      
<member><name>newsletter</name><value><string>§model.Data.Newsletter.Value§</string></value></member>
§endif§
§if(model.Data.AcceptContract.HasValue)§      
<member><name>accept_contract</name><value><string>§model.Data.AcceptContract.Value§</string></value></member>
§endif§
§if(model.Data.ExtraParameters)§      
        <member><name>extra_parameters</name><value>
            <struct>
                <member><name>birth_department</name><value><string>§model.Data.ExtraParameters.BirthDepartment§</string></value></member>
                <member><name>birth_city</name><value><string>§model.Data.ExtraParameters.BirthCity§</string></value></member>
                <member><name>birth_country</name><value><string>§model.Data.ExtraParameters.BirthCountryValue§</string></value></member>
                <member><name>birth_date</name><value><string>§model.Data.ExtraParameters.BirthDateValue§</string></value></member>
            </struct>
            </value>
        </member>
§endif§
   </struct>
</param>
      </params>
   </methodCall>";

            var methodResponse = await MethodResponse(requestTemplate, contactCreateFormDescription);
            var result = ParseContactReturn(methodResponse);
            return result;
        }

        public async Task<bool> CanAssociateDomainAsync(CanAssociateDomainInput input)
        {
            /*
             var association_spec = {
...   domain: 'mydomain.fr',
...   owner: true,
...   admin: true}
> api.methodCall('contact.can_associate_domain',
...    [apikey, 'FLN123-GANDI', association_spec],
...    function (error, value) {
...        console.dir(value)
... })
    */

            var requestTemplate = @"
   <methodCall>
    <methodName>contact.can_associate_domain</methodName>
    <params>
      <param>
         <value><string>§model.ApiKey§</string></value>
         </param>
        <param>
             <value><string>§model.Data.Handle§</string></value>
         </param>
<param>
   <struct>
       <member><name>domain</name><value><string>§model.Data.Input.Domain§</string></value></member>
   </struct>
</param>
<param></param>
      </params>
   </methodCall>";

            var methodResponse = await MethodResponse(requestTemplate, input);

            int data = methodResponse.methodResponse.@params.param.value.@boolean;
            if (data == 1)
            {
                return true;
            }
            return false;
        }

      
    }
}