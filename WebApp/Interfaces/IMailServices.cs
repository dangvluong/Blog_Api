namespace WebApp.Interfaces
{
    public interface IMailServices
    {
        Task SendEmailResetPassword(string addressTo, string resetPasswordToken, ILogger logger);
    }
}
