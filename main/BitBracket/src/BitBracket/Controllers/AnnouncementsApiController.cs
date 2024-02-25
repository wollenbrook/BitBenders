using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsApiController : ControllerBase
{
    private readonly IAnnouncementRepository _announcementRepo;
    private readonly IEmailService _emailService;
    private readonly UserManager<IdentityUser> _userManager;
    private const string AdminKey = "YourAdminKey";

    public AnnouncementsApiController(IAnnouncementRepository announcementRepo, 
                                      IEmailService emailService, 
                                      UserManager<IdentityUser> userManager)
    {
        _announcementRepo = announcementRepo;
        _emailService = emailService;
        _userManager = userManager;
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
        if (adminKey != AdminKey)
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

                await _emailService.SendEmailAsync(user.Email, "New Announcement published on the BitBracketApp", templateData);

            }

            return Ok(new { message = "Announcement created and emails sent successfully." });
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error - could not create the announcement or send emails.");
        }
    }

}
