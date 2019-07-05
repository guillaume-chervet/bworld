using System.Threading.Tasks;

namespace Demo.Email
{
    public interface IEmailService
    {
        Task SendAsync(MailMessage message);
    }
}