using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.Extensions.Logging;

namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnnouncementsApiController : ControllerBase
    {
        private readonly IUserAnnouncementRepository _announcementRepo;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IBitUserRepository _bitUserRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserAnnouncementsApiController> _logger;

        public UserAnnouncementsApiController(
            IUserAnnouncementRepository announcementRepo, 
            UserManager<IdentityUser> userManager, 
            ITournamentRepository tournamentRepository,
            IBitUserRepository bitUserRepository, 
            IEmailService emailService,
            ILogger<UserAnnouncementsApiController> logger)
        {
            _announcementRepo = announcementRepo;
            _userManager = userManager;
            _tournamentRepository = tournamentRepository;
            _bitUserRepository = bitUserRepository;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet("Tournaments")]
        public async Task<IActionResult> GetTournamentsByOwner()
        {
            var userId = _userManager.GetUserId(User);
            var owner = _bitUserRepository.GetBitUserByEntityId(userId);
            
            if (owner == null)
            {
                return NotFound("User not found");
            }

            var tournaments = await _tournamentRepository.GetAllByOwnerId(owner.Id );

            if (tournaments == null)
            {
                return NotFound("No tournaments found for this user");
            }

            return Ok(tournaments);
        }

        [HttpGet("Drafts")]
        [Authorize]
        public async Task<IActionResult> GetDraftAnnouncements()
        {
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                return NotFound("User not found.");
            }

            var announcements = await _announcementRepo.GetByUserIdAndStatus(bitUser.Id.ToString(), true);
            return Ok(announcements);
        }

        [HttpGet("Publish")]
        [Authorize]
        public async Task<IActionResult> GetPublishAnnouncements()
        {
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                return NotFound("User not found.");
            }

            var announcements = await _announcementRepo.GetByUserIdAndStatus(bitUser.Id.ToString(), false);
            return Ok(announcements);
        }

        [HttpPost("Publish/{id}")]
        [Authorize]
        public async Task<IActionResult> PublishAnnouncement(int id)
        {
            _logger.LogInformation($"Attempting to publish announcement with ID: {id}");
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", userId);
                return Unauthorized("User not found.");
            }

            var announcement = await _announcementRepo.GetByIdAsync(id);
            if (announcement == null)
            {
                _logger.LogWarning("Announcement with ID: {Id} not found.", id);
                return NotFound("Announcement not found.");
            }

            if (announcement.Owner != bitUser.Id)
            {
                _logger.LogWarning("Unauthorized access attempt by user {UserId} for announcement {AnnouncementId}", userId, id);
                return Unauthorized("Unauthorized access.");
            }

            announcement.IsDraft = false;
            await _announcementRepo.UpdateAsync(announcement);
            _logger.LogInformation("Announcement with ID: {Id} published successfully.", id);
            return Ok("Announcement published successfully.");
        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> CreateAnnouncement([FromBody] UserAnnouncement announcement)
        {
            var userIdentityId = _userManager.GetUserId(User);
            var userEntity = _bitUserRepository.GetBitUserByEntityId(userIdentityId);

            if (userEntity == null)
            {
                return Unauthorized("User must be logged in.");
            }
            announcement.Owner = userEntity.Id;
            // announcement.Author = user.UserName;
            announcement.CreationDate = DateTime.UtcNow;

            await _announcementRepo.AddAsync(announcement);
            // After adding the announcement, send notifications to opted-in users
            await NotifyOptedInUsers(announcement);
            return Ok(new { message = "Announcement created successfully.", announcement });
        }
        private async Task NotifyOptedInUsers(UserAnnouncement announcement)
        {
            var bitUsers = _bitUserRepository.GetOptedInUsers();
            foreach (var bitUser in bitUsers)
            {
                var identityUser = await _userManager.FindByIdAsync(bitUser.AspnetIdentityId);
                if (identityUser != null && !string.IsNullOrEmpty(identityUser.Email) && identityUser.EmailConfirmed)
                {
                    var templateData = new
                    {
                        title = announcement.Title,
                        description = announcement.Description,
                        author = announcement.Author,
                        time = announcement.CreationDate.ToString("g")
                    };

                    await _emailService.SendEmailAsync(identityUser.Email, "New Announcement at BitBracketApp!", templateData);
                }
            }
        }

        [HttpGet("Published")]
        public async Task<IActionResult> GetPublishedAnnouncements()
        {
            var publishedAnnouncements = await _announcementRepo.GetPublishedAnnouncementsAsync();
            if (publishedAnnouncements == null || !publishedAnnouncements.Any())
            {
                return NotFound("No published announcements found.");
            }

            // Modify to send the tournament name along with each announcement
            var response = publishedAnnouncements.Select(ua => new 
            {
                ua.Id,
                ua.Title,
                ua.Description,
                ua.Author,
                ua.CreationDate,
                TournamentName = ua.Tournament?.Name ?? "No Tournament", // Handle possible null Tournament
                ua.IsDraft
            });

            return Ok(response);
        }

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateAnnouncement(int id, [FromBody] UserAnnouncement updatedAnnouncement)
        {
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                return Unauthorized("User not found.");
            }

            var announcement = await _announcementRepo.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound("Announcement not found.");
            }

            if (announcement.Owner != bitUser.Id)
            {
                return Unauthorized("Unauthorized access.");
            }

            // Update fields
            announcement.Title = updatedAnnouncement.Title;
            announcement.Description = updatedAnnouncement.Description;
            announcement.TournamentId = updatedAnnouncement.TournamentId;
            announcement.IsDraft = updatedAnnouncement.IsDraft;

            await _announcementRepo.UpdateAsync(announcement);
            return Ok("Announcement updated successfully.");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {
            _logger.LogInformation($"Attempting to delete announcement with ID: {id}");
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                _logger.LogWarning("User not found for ID: {UserId}", userId);
                return Unauthorized("User not found.");
            }

            var announcement = await _announcementRepo.GetByIdAsync(id);
            if (announcement == null)
            {
                _logger.LogWarning("Announcement with ID: {Id} not found for deletion.", id);
                return NotFound("Announcement not found.");
            }

            if (announcement.Owner != bitUser.Id)
            {
                _logger.LogWarning("Unauthorized deletion attempt by user {UserId} for announcement {AnnouncementId}", userId, id);
                return Unauthorized("Unauthorized access.");
            }

            await _announcementRepo.DeleteAsync(id);
            _logger.LogInformation("Announcement with ID: {Id} deleted successfully.", id);
            return Ok("Announcement deleted successfully.");
        }

        [HttpGet("GetOptInConfirmation")]
        [Authorize]
        public async Task<IActionResult> GetOptInConfirmation()
        {
            var userId = _userManager.GetUserId(User);
            var user = _bitUserRepository.GetBitUserByEntityId(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { OptInConfirmation = user.OptInConfirmation });
        }

        [HttpPost("UpdateOptInConfirmation")]
        [Authorize]
        public async Task<IActionResult> UpdateOptInConfirmation([FromBody] bool optIn)
        {
            var userId = _userManager.GetUserId(User);
            var user = _bitUserRepository.GetBitUserByEntityId(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.OptInConfirmation = optIn;
             _bitUserRepository.AddOrUpdate(user);

            return Ok("Opt-In confirmation updated successfully.");
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncementById(int id)
        {
            var userId = _userManager.GetUserId(User);
            var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);

            if (bitUser == null)
            {
                return Unauthorized("User not found.");
            }

            var announcement = await _announcementRepo.GetByIdAsync(id);
            if (announcement == null)
            {
                return NotFound("Announcement not found.");
            }

            if (announcement.Owner != bitUser.Id)
            {
                return Unauthorized("Unauthorized access.");
            }

            return Ok(announcement);
        }
    }
}