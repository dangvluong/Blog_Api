using System.Net;
using System.Net.Mail;

namespace WebApp.Services
{
    public static class MailServices
    {
        public static async Task SendEmailResetPassword(IConfiguration configuration, string addressTo, string resetPasswordToken, ILogger logger)
        {
            IConfigurationSection section = configuration.GetSection("Email:Outlook");
            MailAddress emailFrom = new MailAddress(section["address"]);
            MailAddress emailTo = new MailAddress(addressTo.Trim());

            using (MailMessage message = new MailMessage(emailFrom, emailTo))
            {
                message.IsBodyHtml = true;
                message.Body = $"Vui lòng click vào <a href=\"https://localhost:7211/account/resetpassword?email={addressTo}&token={resetPasswordToken}\">ĐÂY</a> để thiết lập lại mật khẩu của bạn. ";
                message.Subject = "CẬP NHẬT MẬT KHẨU";
                using (SmtpClient client = new SmtpClient(section["host"], int.Parse(section["port"]))
                {
                    Credentials = new NetworkCredential(section["address"], section["password"]),
                    EnableSsl = true
                })
                {
                    try
                    {
                        logger.LogInformation($"Send email reset password to {addressTo}");
                        await client.SendMailAsync(message);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Unhandle exception");
                    }
                }
            }
        }
    }
}
