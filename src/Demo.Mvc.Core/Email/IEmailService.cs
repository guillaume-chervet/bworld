using System.Threading.Tasks;

namespace Demo.Mvc.Core.Email
{
    public interface IEmailService
    {
        Task SendAsync(MailMessage message);
    }
}