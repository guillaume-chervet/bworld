using System.Net;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Newtonsoft.Json;

namespace Demo.Email
{
	public class EmailSendGridService : IEmailService
	{
	    private readonly EmailConfig _emailConfig;

	    private string _sender;

		public EmailSendGridService(IOptions<EmailConfig> otpions)
		{
		    _emailConfig = otpions.Value;
		    _sender = "bworld";
		}

		public Task SendAsync(MailMessage message)
        {
            return ConfigSendGridasyncAsync(message);
        }

        private async Task ConfigSendGridasyncAsync(MailMessage message)
        {
            var client = new SendGridClient(_emailConfig.ApiKey);
            var from = new EmailAddress("noreply@bworld.fr", string.Concat("[", _sender, "]"));
            var subject = message.Subject;
            var to = new EmailAddress(message.Destination);
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            
            if(response.StatusCode != System.Net.HttpStatusCode.Accepted)
            {
                throw new Exception("Send mail error " + JsonConvert.SerializeObject(message));
            }
        }
    }

}
