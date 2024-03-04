using BitBracket.DAL.Abstract;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace BitBracket.DAL.Concrete
{
    public class EmailService : IEmailService
    {
        private readonly string _sendGridKey;

        public EmailService(string sendGridKey)
        {
            _sendGridKey = sendGridKey;
        }

        public async Task SendEmailAsync(string toEmail, string subject, object templateData)
        {
            var client = new SendGridClient(_sendGridKey);
            var from = new EmailAddress("BitBracketApp@gmail.com", "BitBracketApp");
            var to = new EmailAddress(toEmail);
            
            var msg = new SendGridMessage()
            {
                From = from,
                Subject = subject, // Ensure the subject is being set here
                TemplateId = "d-b3807854b3344976a002acc81475dc12" // The SendGrid template ID
            };
            
            msg.AddTo(to);
            msg.SetTemplateData(templateData); // Sets the data for the template

            await client.SendEmailAsync(msg); // Sends the email
        }
    }
}


