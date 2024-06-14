using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Microsoft.Extensions.Configuration;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsApiController : ControllerBase
{
    private readonly IAnnouncementRepository _announcementRepo;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService; 
    private readonly UserManager<IdentityUser> _userManager;
    private readonly string _adminKey;

    public AnnouncementsApiController(IAnnouncementRepository announcementRepo, 
                                      IEmailService emailService, 
                                      ISmsService smsService, 
                                      UserManager<IdentityUser> userManager,
                                      IConfiguration configuration)
    {
        _announcementRepo = announcementRepo;
        _emailService = emailService;
        _smsService = smsService;
        _userManager = userManager;
        _adminKey = configuration["AdminKey"]; // Retrieve the AdminKey from the configuration
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var announcements = await _announcementRepo.GetAllAsync();
        return Ok(announcements);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Announcement announcement, [FromQuery] string adminKey)
    {
        if (adminKey != _adminKey)
        {
            return Unauthorized();
        }

        try
        {
            await _announcementRepo.AddAsync(announcement);

            var users = _userManager.Users.Where(u => u.EmailConfirmed).ToList();
            foreach (var user in users)
            {
                var templateData = new
                {
                    title = announcement.Title,
                    description = announcement.Description,
                    author = announcement.Author,
                    time = announcement.CreationDate.ToString("g")
                };

                // Send Email
                if (!string.IsNullOrEmpty(user.Email))
                {
                    await _emailService.SendEmailAsync(user.Email, "New Announcement published on the BitBracketApp!", templateData);
                }

                // Send SMS
                if (!string.IsNullOrEmpty(user.PhoneNumber))
                {
                    var smsMessage = $"New Announcement: {announcement.Title}. Check BitBracketApp for details.";
                    await _smsService.SendSmsAsync(user.PhoneNumber, smsMessage);
                }
            }

            return Ok(new { message = "Announcement created and notifications sent successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error - could not create the announcement or send notifications.");
        }
    }
}
