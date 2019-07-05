using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SendGrid;

namespace Demo.Email.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task SimplePlaintextEmail()
        {
            try
            {
                var emailConfig = Options.Create(new EmailConfig() { ApiKey = "toto" });
                EmailSendGridService emailService = new EmailSendGridService(emailConfig);

                await emailService.SendAsync(new MailMessage()
                {
                    Body = "Test pas très Unitaire",
                    Destination = "guillaume.chervet@gmail.com",
                    Subject = "Test pas très Unitaire"
                });

            } catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         

        }


    }
}
