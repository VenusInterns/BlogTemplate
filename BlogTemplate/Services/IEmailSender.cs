using System.Threading.Tasks;

namespace BlogTemplate._1.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
