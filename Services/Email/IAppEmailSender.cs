using MP_Backend.Models;

namespace MP_Backend.Services.Email
{
    public interface IAppEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendOrderConfimationEmail(Order order, string toEmail, string subject, string message);
    }
}
