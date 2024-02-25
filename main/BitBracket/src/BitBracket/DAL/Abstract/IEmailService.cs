namespace BitBracket.DAL.Abstract
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, object templateData);
    }
}

