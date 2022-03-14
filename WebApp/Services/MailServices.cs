using System.Net;
using System.Net.Mail;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class MailServices : IMailServices
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MailServices(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendEmailResetPassword(string addressTo, string resetPasswordToken, ILogger logger)
        {
            IConfigurationSection section = _configuration.GetSection("Email:Outlook");
            MailAddress emailFrom = new MailAddress(section["address"]);
            MailAddress emailTo = new MailAddress(addressTo.Trim());
            var request = _httpContextAccessor.HttpContext.Request;
            var domain = $"{request.Scheme}://{request.Host}";
            using (MailMessage message = new MailMessage(emailFrom, emailTo))
            {
                message.IsBodyHtml = true;
                message.Body = $"Vui lòng click vào <a href=\"{domain}/account/resetpassword?email={addressTo}&token={resetPasswordToken}\">ĐÂY</a> để thiết lập lại mật khẩu của bạn. ";
                message.Subject = "CẬP NHẬT MẬT KHẨU";
                using (SmtpClient client = new SmtpClient(section["host"], int.Parse(section["port"])))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(section["address"], section["password"]);
                    client.EnableSsl = true;
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
