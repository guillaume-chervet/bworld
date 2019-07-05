using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Email
{
    public class EmailConsoleService : IEmailService
    {
        private readonly ILogger<EmailConsoleService> logger;

        public EmailConsoleService (ILogger<EmailConsoleService> logger)
        {
            this.logger = logger;
        }

        public async Task SendAsync(MailMessage message)
        {
            await Task.Delay(200);
            logger.LogInformation(JsonConvert.SerializeObject(message));
        }
    }
}
