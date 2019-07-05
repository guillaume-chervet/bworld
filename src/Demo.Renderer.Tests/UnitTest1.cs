using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Renderer.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            /*IF conditionnel*/
            string template = "Bonjour §model.Name§, ceci est un §if(model.IsMail)§" +
                              "mail" +
                              "   §else§" +
                              "sms" +
                              "  §endif§!";
            string result = new StringTemplateRenderer().Render(template, new { Name = "Guillaume", IsMail = true });

            Console.WriteLine(result); // “Bonjour Guillaume, ceci est un sms!”

        }




        [TestMethod]
        public void TestMethod2()
        {
            /*IF conditionnel*/

            string template = @"Bonjour §model.UserName§,<br />
<br />
§if(model.IsMail)§
                              mail
                              §else§
                             sms
                               §endif§!

    < br />
	Titre: §model.Title§<br />
§if(model.IsNotAuthenticated)§
	Email: §model.Sender.Email§<br />
	Téléphone: §model.Sender.Phone§<br />
§endif§
	Message: <br />
§model.Message§<br />
	<br />
§if(model.MessageUrl)§
	Message disponible sur le site<a href='§model.MessageUrl§'>§model.SiteName§</a><br /><br />
§endif§
Cordialement,<br />
Service Client §model.SiteName§,<br />
<a href = '§model.SiteUrl§' >§model.SiteUrl§</a>";
            string result = new StringTemplateRenderer().Render(template, new { Name = "Guillaume", IsMail = true , IsReply =false});

            Console.WriteLine(result); // “Bonjour Guillaume, ceci est un sms!”

        }
    }
}
