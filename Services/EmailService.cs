using MimeKit;
using MailKit.Net.Smtp;

namespace TaskManagementAPI.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger
        )
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(string recipientEmail, string subject, string body)
        {
            try
            {
                // Create a new MimeMessage.
                var message = new MimeMessage();

                // Set the sender and recipient addresses.
                message.From.Add(new MailboxAddress("Your Name", "no-reply@example.com"));
                message.To.Add(new MailboxAddress("", recipientEmail)); // Leave the recipient name empty

                // Set the subject and body of the email.
                message.Subject = subject;

                // Create a text part for the email body.
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;

                // Attach the text part to the message.
                message.Body = bodyBuilder.ToMessageBody();

                // Create a new SmtpClient instance.
                using var smtpClient = new SmtpClient();

                // Set the SMTP server and port from the appsettings.json file.
               await smtpClient.ConnectAsync(_configuration["MailTrap:Host"], int.Parse(_configuration["MailTrap:Port"]!), false);

                // Get the username and password from the appsettings.json file.
                string username = _configuration["MailTrap:Username"]!;
                string password = _configuration["MailTrap:Password"]!;

                // Authenticate with the SMTP server.
              await  smtpClient.AuthenticateAsync(username, password);

                // Send the email.
              await  smtpClient.SendAsync(message);

                // Disconnect from the SMTP server.
               await smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }


        public string EmailVerificationTemplate(string verificationLink)
        {
            return $@"
        <!DOCTYPE html>
        <html lang=""en"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Email Verification</title>
        </head>
        <body>
            <h1>Email Verification</h1>
            <p>Dear user,</p>
            <p>Thank you for registering with our service. To verify your email address, please click the link below:</p>
            <p><a href=""{verificationLink}"">Verify your email</a></p>
            <p>If you didn't request this verification, please ignore this email.</p>
            <p>Regards,<br>Your Company Name</p>
        </body>
        </html>
        ";
        }
    }
}