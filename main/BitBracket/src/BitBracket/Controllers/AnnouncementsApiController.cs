using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class AnnouncementsApiController : ControllerBase
{
    private readonly IAnnouncementRepository _announcementRepo;
    private const string AdminKey = "YourAdminKey"; // Admin Key

    public AnnouncementsApiController(IAnnouncementRepository announcementRepo)
    {
        _announcementRepo = announcementRepo;
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
            // Assuming AddAsync sets the ID of the announcement after saving it
            return Ok(announcement); // Return the created announcement object
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal Server Error - could not create the announcement.");
        }
    }

}
