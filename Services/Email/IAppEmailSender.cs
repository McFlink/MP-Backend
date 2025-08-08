using MP_Backend.Models;

namespace MP_Backend.Services.Email
{
    public interface IAppEmailSender
    {
        Task SendEmailConfirmationLinkAsync(string toEmail, string subject, string message);
        Task SendOrderConfimationEmailToUser(Order order, string toEmail, string subject, string message);
        Task SendOrderConfirmationEmailToAdmin(Order order, string toEmail, string subject, string message);
        Task SendEmailNotificationOnNewRetailerRegister(UserProfile newRetailer);
        Task SendEmailNotificationOnNewCustomerRegister(UserProfile newCustomer);
    }
}
