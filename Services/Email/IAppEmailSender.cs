namespace MP_Backend.Services.Email
{
    public interface IAppEmailSender
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
