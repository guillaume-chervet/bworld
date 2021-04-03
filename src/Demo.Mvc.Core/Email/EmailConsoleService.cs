using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Email
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
