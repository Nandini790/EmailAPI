using EmailAPI.IServices;
using EmailAPI.Models;
using EmailAPI.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _settings;

        public EmailService(IOptions<SendGridSettings> options)
        {
            _settings = options.Value;
        }

        public async Task<bool> SendEmailAsync(ContactForm form)
        {
            var client = new SendGridClient(_settings.ApiKey);

            var from = new EmailAddress(_settings.SenderEmail, _settings.SenderName);
            var to = new EmailAddress(_settings.ReceiverEmail);

            var subject = $"New Contact Request from {form.Name}";
            var plainText = $"Email: {form.Email}\n\nMessage:\n{form.Message}";
            var htmlContent = $@"
                <strong>Name:</strong> {form.Name}<br/>
                <strong>Email:</strong> {form.Email}<br/><br/>
                <strong>Message:</strong><br/>
                {form.Message}
            ";

            var msg = MailHelper.CreateSingleEmail(
                from, to, subject, plainText, htmlContent);

            var response = await client.SendEmailAsync(msg);

            return response.IsSuccessStatusCode;
        }
    }
}
