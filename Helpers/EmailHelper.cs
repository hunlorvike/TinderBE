using System;
using System.Net;
using System.Net.Mail;

namespace Tinder_Admin.Helpers
{
    public partial class EmailHelper
    {
        private static readonly string HostAddress = "smtp.gmail.com";
        private static readonly string FromEmail = "viethung0106.developer@gmail.com";
        private static readonly string Password = "xwkvhbfbfnhsgugl";
        private static readonly int Port = 587;

        public static bool SendVerifyEmail(string recipientEmail, string subject, string messageBody, bool isBodyHtml = false)
        {
            try
            {
                using (var client = new SmtpClient(HostAddress, Port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(FromEmail, Password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(FromEmail),
                        Subject = subject,
                        Body = messageBody,
                        IsBodyHtml = isBodyHtml
                    };

                    mailMessage.To.Add(new MailAddress(recipientEmail));

                    client.Send(mailMessage);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                return false;
            }
        }
    }
}

