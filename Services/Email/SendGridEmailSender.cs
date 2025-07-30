//using DocumentFormat.OpenXml.Drawing.Charts;
using MP_Backend.Helpers;
using MP_Backend.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MP_Backend.Services.Email
{
    public class SendGridEmailSender : IAppEmailSender
    {
        private readonly IConfiguration _config;

        public SendGridEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailConfirmationLinkAsync(string toEmail, string subject, string confirmationLink)
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

        public async Task SendOrderConfimationEmailToUser(Order order, string toEmail, string subject, string message)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(_config["SendGrid:From"], "MP Fishing Supply");
            var to = new EmailAddress(toEmail);
            var plainTextContent = subject;

            var profile = order.UserProfile;
            var billingAddress = profile.BillingAddress ?? "Ej angiven";
            var deliveryAddress = profile.Address ?? "Ej angiven";
            var company = profile.CompanyName ?? "Ej angivet företag";
            var orgNr = profile.OrganizationNumber ?? "Ej angivet org.nr";

            // Beräkningar (alla priser exklusive moms först)
            decimal shippingFeeExclVat = PriceConstants.ShippingFee;
            decimal itemTotalExclVat = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            decimal totalExclVat = itemTotalExclVat + shippingFeeExclVat;

            // Moms beräknas på nettosumman (produkter + frakt)
            decimal vatRate = PriceConstants.VatRate; // 25%
            decimal vat = totalExclVat * vatRate;
            decimal totalInclVat = totalExclVat + vat;

            // För presentation med moms
            decimal shippingFeeInclVat = shippingFeeExclVat * (1 + vatRate);
            decimal itemTotalInclVat = itemTotalExclVat * (1 + vatRate);

            var htmlContent = $@"
            <html>
              <body style=""font-family: sans-serif; background-color: #f9f9f9; padding: 20px;"">
                <div style=""max-width: 700px; margin: auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                  <h2 style=""color: #2a7f62;"">Tack för din beställning!</h2>

                  <p><strong>Ordernummer:</strong> {order.OrderNumber}<br/>
                     <strong>Orderdatum:</strong> {order.CreatedAt:yyyy-MM-dd}</p>

                  <h3 style=""margin-top: 30px;"">Kundinformation</h3>
                  <p>
                    <strong>{company}</strong><br/>
                    Kundnummer: {profile.CustomerNumber}<br/>
                    Org.nr: {orgNr}<br/>
                    E-post: {profile.User?.Email ?? "okänd"}
                  </p>

                  <h3 style=""margin-top: 20px;"">Fakturaadress</h3>
                  <p>{billingAddress}</p>

                  <h3 style=""margin-top: 20px;"">Leveransadress</h3>
                  <p>{deliveryAddress}</p>

                  <h3 style=""margin-top: 30px;"">Orderinnehåll</h3>
                  <table style=""width: 100%; border-collapse: collapse;"">
                    <thead>
                      <tr style=""background-color: #eeeeee;"">
                        <th style=""text-align: left; padding: 8px;"">Produkt</th>
                        <th style=""text-align: right; padding: 8px;"">Variant</th>
                        <th style=""text-align: right; padding: 8px;"">Antal</th>
                        <th style=""text-align: right; padding: 8px;"">Pris exkl. moms</th>
                        <th style=""text-align: right; padding: 8px;"">Pris inkl. moms</th>
                        <th style=""text-align: right; padding: 8px;"">Summa inkl. moms</th>
                      </tr>
                    </thead>
                    <tbody>
                      {string.Join("", order.Items.Select(i =>
                                              $@"<tr>
                                <td style=""padding: 8px;"">{i.ProductVariant.Product.Name}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.ProductVariant.Name}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.Quantity}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.UnitPrice:N2} kr</td>
                                <td style=""text-align: right; padding: 8px;"">{(i.UnitPrice * (1 + vatRate)):N2} kr</td>
                                <td style=""text-align: right; padding: 8px;"">{(i.UnitPrice * i.Quantity * (1 + vatRate)):N2} kr</td>
                            </tr>"))}
                    </tbody>
                  </table>

                  <p style=""text-align: right; margin-top: 20px;"">
                    <strong>Delsumma exkl. moms:</strong> {itemTotalExclVat:N2} kr<br/>
                    <strong>Delsumma inkl. moms:</strong> {itemTotalInclVat:N2} kr<br/>
                    <strong>Frakt exkl. moms:</strong> {shippingFeeExclVat:N2} kr<br/>
                    <strong>Frakt inkl. moms:</strong> {shippingFeeInclVat:N2} kr<br/>
                    <strong>Moms (25%):</strong> {vat:N2} kr<br/>
                    <strong style=""font-size: 18px;"">Totalt att betala: {totalInclVat:N2} kr</strong>
                  </p>

                  <p style=""margin-top: 30px;"">Er order skickas inom 48 timmar.</p>

                  <p style=""margin-top: 20px;"">Vid frågor om din beställning, vänligen kontakta oss på<br/>
                  <strong>info@mpfishingsupply.se</strong>. Bifoga ordernummer.</p>

                  <p>Detta mail går ej att svara på.</p>

                  <p style=""margin-top: 40px;"">Hälsningar,<br/><strong>MP Fishing Supply</strong></p>
                </div>
              </body>
            </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await client.SendEmailAsync(msg);
        }

        public async Task SendOrderConfirmationEmailToAdmin(Order order, string toEmail, string subject, string message)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var profile = order.UserProfile;
            var billingAddress = profile.BillingAddress ?? "Ej angiven";
            var deliveryAddress = profile.Address ?? "Ej angiven";
            var company = profile.CompanyName ?? "Ej angivet företag";
            var orgNr = profile.OrganizationNumber ?? "Ej angivet org.nr";

            // Beräkningar (alla priser exklusive moms först)
            decimal shippingFeeExclVat = PriceConstants.ShippingFee;
            decimal itemTotalExclVat = order.Items.Sum(i => i.UnitPrice * i.Quantity);
            decimal totalExclVat = itemTotalExclVat + shippingFeeExclVat;

            // Moms beräknas på nettosumman (produkter + frakt)
            decimal vatRate = PriceConstants.VatRate; // 25%
            decimal vat = totalExclVat * vatRate;
            decimal totalInclVat = totalExclVat + vat;

            // För presentation med moms
            decimal shippingFeeInclVat = shippingFeeExclVat * (1 + vatRate);
            decimal itemTotalInclVat = itemTotalExclVat * (1 + vatRate);

            var from = new EmailAddress(_config["SendGrid:From"], $"{profile.CompanyName} - Ny Order");
            var to = new EmailAddress(toEmail);
            var plainTextContent = subject;
            var htmlContent = $@"
            <html>
              <body style=""font-family: sans-serif; background-color: #f9f9f9; padding: 20px;"">
                <div style=""max-width: 700px; margin: auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);"">
                  <h2 style=""color: #2a7f62;"">{company} la just en order!</h2>

                  <p><strong>Ordernummer:</strong> {order.OrderNumber}<br/>
                     <strong>Orderdatum:</strong> {order.CreatedAt:yyyy-MM-dd}</p>

                  <h3 style=""margin-top: 30px;"">Kundinformation</h3>
                  <p>
                    <strong>{company}</strong><br/>
                    Kundnummer: {profile.CustomerNumber}<br/>
                    Org.nr: {orgNr}<br/>
                    E-post: {profile.User?.Email ?? "okänd"}
                  </p>

                  <h3 style=""margin-top: 20px;"">Kontaktperson</h3>
                  <p>{profile.FirstName} {profile.LastName}</p>

                  <h3 style=""margin-top: 20px;"">Fakturaadress</h3>
                  <p>{billingAddress}</p>

                  <h3 style=""margin-top: 20px;"">Leveransadress</h3>
                  <p>{deliveryAddress}</p>

                  <h3 style=""margin-top: 30px;"">Orderinnehåll</h3>
                  <table style=""width: 100%; border-collapse: collapse;"">
                    <thead>
                      <tr style=""background-color: #eeeeee;"">
                        <th style=""text-align: left; padding: 8px;"">Produkt</th>
                        <th style=""text-align: right; padding: 8px;"">Variant</th>
                        <th style=""text-align: right; padding: 8px;"">Antal</th>
                        <th style=""text-align: right; padding: 8px;"">Pris exkl. moms</th>
                        <th style=""text-align: right; padding: 8px;"">Pris inkl. moms</th>
                        <th style=""text-align: right; padding: 8px;"">Summa inkl. moms</th>
                      </tr>
                    </thead>
                    <tbody>
                      {string.Join("", order.Items.Select(i =>
                                             $@"<tr>
                                <td style=""padding: 8px;"">{i.ProductVariant.Product.Name}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.ProductVariant.Name}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.Quantity}</td>
                                <td style=""text-align: right; padding: 8px;"">{i.UnitPrice:N2} kr</td>
                                <td style=""text-align: right; padding: 8px;"">{(i.UnitPrice * (1 + vatRate)):N2} kr</td>
                                <td style=""text-align: right; padding: 8px;"">{(i.UnitPrice * i.Quantity * (1 + vatRate)):N2} kr</td>
                            </tr>"))}
                    </tbody>
                  </table>

                  <p style=""text-align: right; margin-top: 20px;"">
                    <strong>Delsumma exkl. moms:</strong> {itemTotalExclVat:N2} kr<br/>
                    <strong>Delsumma inkl. moms:</strong> {itemTotalInclVat:N2} kr<br/>
                    <strong>Frakt exkl. moms:</strong> {shippingFeeExclVat:N2} kr<br/>
                    <strong>Frakt inkl. moms:</strong> {shippingFeeInclVat:N2} kr<br/>
                    <strong>Moms (25%):</strong> {vat:N2} kr<br/>
                    <strong style=""font-size: 18px;"">Totalt att betala: {totalInclVat:N2} kr</strong>
                  </p>

                  <p style=""margin-top: 40px;"">Automatiskt epost vid ny order från<br/><strong>mpfishingsupply.se</strong></p>
                </div>
              </body>
            </html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
