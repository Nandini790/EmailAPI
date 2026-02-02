using EmailAPI.Models;

namespace EmailAPI.IServices
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(ContactForm form);
    }
}
