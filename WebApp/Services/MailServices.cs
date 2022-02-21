using System.Net.Mail;

namespace WebApp.Services
{
    public static class MailServices
    {
        public static void SendEmail(string emailFrom, string emailTo, string subject, string body, SmtpClient client)
        {
            MailAddress addressFrom = new MailAddress(emailFrom);
            MailAddress addressTo = new MailAddress(emailTo.Trim());
            MailMessage message = new MailMessage(addressFrom, addressTo);
            message.IsBodyHtml = true;
            message.Body = body;
            message.Subject = subject;
            client.SendCompleted += (s, e) =>
            {
                message.Dispose();
                client.Dispose();
            };
            client.SendMailAsync(message);
        }
    }
}
