using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace SmartExamSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendStudentWelcomeEmailAsync(string toEmail, string studentName, string password, string loginUrl)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var smtpServer = emailSettings["SmtpServer"];
            var smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            var senderEmail = emailSettings["SenderEmail"];
            var senderName = emailSettings["SenderName"] ?? "Smart Exam System";
            var senderPassword = emailSettings["Password"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress(studentName, toEmail));
            message.Subject = "Welcome to Smart Exam System - Your Account Details";

            var htmlBody = BuildWelcomeEmailHtml(studentName, toEmail, password, loginUrl);
            message.Body = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(senderEmail, senderPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Welcome email sent successfully to {Email}", toEmail);
        }

        private string BuildWelcomeEmailHtml(string studentName, string email, string password, string loginUrl)
        {
            return $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
</head>
<body style=""margin:0;padding:0;background-color:#f0f2f5;font-family:'Segoe UI',Roboto,'Helvetica Neue',Arial,sans-serif;"">
    <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#f0f2f5;padding:40px 20px;"">
        <tr>
            <td align=""center"">
                <table role=""presentation"" width=""600"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#ffffff;border-radius:16px;overflow:hidden;box-shadow:0 4px 24px rgba(0,0,0,0.08);"">
                    
                    <!-- Header -->
                    <tr>
                        <td style=""background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);padding:40px 40px 32px;text-align:center;"">
                            <div style=""font-size:32px;margin-bottom:12px;"">🎓</div>
                            <h1 style=""color:#ffffff;font-size:24px;font-weight:700;margin:0 0 8px;letter-spacing:-0.5px;"">Smart Exam System</h1>
                            <p style=""color:rgba(255,255,255,0.85);font-size:14px;margin:0;"">Your Online Examination Portal</p>
                        </td>
                    </tr>

                    <!-- Welcome Message -->
                    <tr>
                        <td style=""padding:40px 40px 24px;"">
                            <h2 style=""color:#1a1a2e;font-size:22px;font-weight:700;margin:0 0 12px;"">Welcome, {studentName}! 👋</h2>
                            <p style=""color:#555;font-size:15px;line-height:1.6;margin:0;"">
                                Your student account has been successfully created by the administrator. 
                                You can now log in to the exam portal using the credentials below.
                            </p>
                        </td>
                    </tr>

                    <!-- Credentials Box -->
                    <tr>
                        <td style=""padding:0 40px 24px;"">
                            <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background:linear-gradient(135deg,#f5f7ff 0%,#ede9fe 100%);border-radius:12px;border:1px solid #e0d4fc;"">
                                <tr>
                                    <td style=""padding:28px 24px;"">
                                        <p style=""color:#667eea;font-size:12px;font-weight:700;text-transform:uppercase;letter-spacing:1.5px;margin:0 0 16px;"">Your Login Credentials</p>
                                        
                                        <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"">
                                            <tr>
                                                <td style=""padding:8px 0;"">
                                                    <span style=""color:#888;font-size:13px;display:inline-block;width:90px;"">📧 Email:</span>
                                                    <span style=""color:#1a1a2e;font-size:15px;font-weight:600;"">{email}</span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style=""padding:8px 0;"">
                                                    <span style=""color:#888;font-size:13px;display:inline-block;width:90px;"">🔑 Password:</span>
                                                    <span style=""color:#1a1a2e;font-size:15px;font-weight:600;"">{password}</span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <!-- CTA Button -->
                    <tr>
                        <td style=""padding:0 40px 16px;text-align:center;"">
                            <a href=""{loginUrl}"" style=""display:inline-block;background:linear-gradient(135deg,#667eea 0%,#764ba2 100%);color:#ffffff;font-size:16px;font-weight:600;text-decoration:none;padding:14px 48px;border-radius:8px;letter-spacing:0.3px;"">
                                Login to Dashboard →
                            </a>
                        </td>
                    </tr>

                    <!-- Login URL fallback -->
                    <tr>
                        <td style=""padding:0 40px 32px;text-align:center;"">
                            <p style=""color:#999;font-size:12px;margin:0;"">
                                Or copy this link: <a href=""{loginUrl}"" style=""color:#667eea;"">{loginUrl}</a>
                            </p>
                        </td>
                    </tr>

                    <!-- Security Notice -->
                    <tr>
                        <td style=""padding:0 40px 32px;"">
                            <table role=""presentation"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""background-color:#fff8e1;border-radius:8px;border:1px solid #ffe082;"">
                                <tr>
                                    <td style=""padding:16px 20px;"">
                                        <p style=""color:#f57c00;font-size:13px;font-weight:600;margin:0 0 4px;"">⚠️ Security Tip</p>
                                        <p style=""color:#795548;font-size:13px;line-height:1.5;margin:0;"">
                                            Please change your password after your first login. Do not share your credentials with anyone.
                                        </p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <!-- Footer -->
                    <tr>
                        <td style=""background-color:#f8f9fb;padding:24px 40px;border-top:1px solid #eee;"">
                            <p style=""color:#999;font-size:12px;line-height:1.6;margin:0;text-align:center;"">
                                This is an automated message from Smart Exam System.<br>
                                If you did not expect this email, please contact your administrator.
                            </p>
                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }
    }
}
