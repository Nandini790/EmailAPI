using EmailAPI.IServices;
using EmailAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace EmailAPI.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("contactLimiter")]
    public class EmailController : ControllerBase
    {
            private readonly IEmailService _emailService;

            public EmailController(IEmailService emailService)
            {
                _emailService = emailService;
            }

            [HttpPost("send")]
            public async Task<IActionResult> Send([FromBody] ContactForm form)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var success = await _emailService.SendEmailAsync(form);

                if (!success)
                    return StatusCode(500, "Failed to send email.");

                return Ok(new { message = "Message sent successfully." });
            }
        }
}
