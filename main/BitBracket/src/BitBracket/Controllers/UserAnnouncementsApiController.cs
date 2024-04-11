using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System;

namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAnnouncementsApiController : ControllerBase
    {
        private readonly IUserAnnouncementRepository _announcementRepo;
        private readonly UserManager<BitUser> _userManager;

        public UserAnnouncementsApiController(IUserAnnouncementRepository announcementRepo, UserManager<BitUser> userManager)
        {
            _announcementRepo = announcementRepo;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Announcement announcement)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return BadRequest("User not found.");
            }

            announcement.BitUserId = user.Id; // Link the announcement to the BitUser
            announcement.Author = user.Username; // Fill the Author field with BitUser's username

            await _announcementRepo.AddAsync(announcement);
            return Ok(new { message = "Announcement created successfully", announcement });
        }

        [HttpGet("drafts")]
        [Authorize]
        public async Task<IActionResult> GetDrafts()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return BadRequest("User not found.");
            }

            var drafts = await _announcementRepo.GetByUserIdAndStatus(user.Id, true);
            return Ok(drafts);
        }

        [HttpGet("published")]
        [Authorize]
        public async Task<IActionResult> GetPublished()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return BadRequest("User not found.");
            }

            var publishedAnnouncements = await _announcementRepo.GetByUserIdAndStatus(user.Id, false);
            return Ok(publishedAnnouncements);
        }

        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestAnnouncement()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) 
            {
                return BadRequest("User not found.");
            }

            var latestAnnouncement = await _announcementRepo.GetLatestAnnouncementAsync(user.Id);
            if (latestAnnouncement == null) return NotFound("No latest announcement found.");

            return Ok(latestAnnouncement);
        }
        
        [HttpPost("OptInConfirmation")]
        public async Task<IActionResult> SetOptInConfirmation([FromBody] OptInModel optInModel)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return BadRequest("User not found.");

            user.OptInConfirmation = optInModel.OptIn;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(new { message = "Opt-in confirmation updated successfully." });
            }

            return BadRequest("Failed to update opt-in confirmation.");
        }

        public class OptInModel
        {
            public bool OptIn { get; set; }
        }

    }
}
