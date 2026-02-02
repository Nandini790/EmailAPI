using System.ComponentModel.DataAnnotations;

namespace EmailAPI.Models
{
    public class ContactForm
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; } = string.Empty;
    }
}

