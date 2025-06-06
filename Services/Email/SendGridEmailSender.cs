


using SendGrid;
using SendGrid.Helpers.Mail;

namespace MP_Backend.Services.Email
{
    public class SendGridEmailSender : IAppEmailSender
    {
        private readonly IConfiguration _config;

        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string confirmationLink)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(_config["SendGrid:From"], "MP Fishing Supply");
            var to = new EmailAddress(toEmail);
            var plainTextContent = subject;
            var htmlContent = $@"
            <html>
              <body style=""font-family: sans-serif; background-color: #f9f9f9; padding: 20px;"">
                <div style=""max-width: 600px; margin: auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                  <h2 style=""color: #2a7f62;"">Välkommen till MP Fishing Supply</h2>
                  <p>Hej!</p>
                  <p style=""padding-bottom: 1rem;"">Klicka på knappen nedan för att bekräfta din e-postadress:</p>
                  <p style=""text-align: center; padding-bottom: 1rem;"">
                    <a href=""{confirmationLink}"" style=""background-color: #2a7f62; color: white; padding: 12px 24px; border-radius: 6px; text-decoration: none;"">Bekräfta e-post</a>
                  </p>
                  <p>Om du inte skapade ett konto hos oss kan du ignorera detta meddelande.</p>
                  <br />
                  <p>Hälsningar,<br /><strong>MP Fishing Supply</strong></p>
                </div>
              </body>
            </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
