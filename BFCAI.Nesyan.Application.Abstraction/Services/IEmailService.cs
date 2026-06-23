using System.Threading.Tasks;

namespace BFCAI.Nesyan.Application.Abstraction.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
