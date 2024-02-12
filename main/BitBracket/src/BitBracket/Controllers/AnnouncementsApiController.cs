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

        await _announcementRepo.AddAsync(announcement);
        return Ok();
    }
}
