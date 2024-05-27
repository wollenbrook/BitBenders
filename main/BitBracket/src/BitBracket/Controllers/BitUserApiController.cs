using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitBracket.Models;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using BitBracket.ExtensionMethods;
using BitBracket.DTO;


namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitUserApiController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBitUserRepository _bitUserRepository;


        public BitUserApiController(UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository)
        {
           
            _userManager = userManager;
            _bitUserRepository = bitUserRepository;
        }

        // GET: api/BitUserApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BitUserDTO>>> GetBitUser()
        {
            var bitUsers = await _bitUserRepository.GetAll()
                .Where(u => u.EmailConfirmedStatus)
                .ToListAsync();
            if (bitUsers == null)
            {
                return NotFound();
            }
            var DtoBitUsers = bitUsers.Select(u => u.ReturnBitUserSearchDTO()).ToList();
            return DtoBitUsers;
        }
        // GET: api/BitUserApi/Search/{username}
        [HttpGet("Search/{username}")]
        public async Task<ActionResult<IEnumerable<BitUser>>> GetBitUserByUsername(string username)
        {
            var bitUsers = await _bitUserRepository.GetAll()
                .Where(u => (u.Username.Contains(username) && u.EmailConfirmedStatus))
                .ToListAsync();
            if (bitUsers == null)
            {
                return NotFound();
            }

            var DtoBitUsers = bitUsers.Select(u => u.ReturnBitUserSearchDTO()).ToList();
            return bitUsers;
        }
        // GET: api/BitUserApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BitUser>> GetBitUser(int id)
        {
            var bitUser = _bitUserRepository.FindById(id);

            if (bitUser == null)
            {
                return NotFound();
            }

            return bitUser;
        }
        // PUT: api/BitUserApi/BioChange/{bio}
        [HttpPut("BioChange/{bio}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBitUserBio(string bio)
        {
            string id = _userManager.GetUserId(User);
            BitUser bitUser = _bitUserRepository.GetBitUserByEntityId(id);
            bitUser.Bio = bio;
            _bitUserRepository.AddOrUpdate(bitUser);
            return Ok();
        }

        [HttpPut("TagChange/{tag}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBitUserTag(string tag)
        {
            string id = _userManager.GetUserId(User);
            BitUser bitUser = _bitUserRepository.GetBitUserByEntityId(id);
            bitUser.Tag = tag;
            _bitUserRepository.AddOrUpdate(bitUser);

            return Ok();
        }


        // POST: api/BitUserApi/Image/{file}
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Image")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                // Read the file content into a byte array
                byte[] profilePictureBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    profilePictureBytes = memoryStream.ToArray();
                }

                // Save the byte array as the user's profile picture
                string id = _userManager.GetUserId(User);
                BitUser bitUser = _bitUserRepository.GetBitUserByEntityId(id);
                bitUser.ProfilePicture = profilePictureBytes;
                _bitUserRepository.AddOrUpdate(bitUser);
                return Ok();


            }

            // Handle the case when no file is uploaded
            return BadRequest();
        }
        // PUT: api/SendFriendRequest/5
        [HttpPut("SendFriendRequest/{id}")]
        public async Task<IActionResult> SendFriendRequest(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser reciver = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            _bitUserRepository.SendFriendRequest(sender, reciver);

            return Ok();
        }
        // PUT: /api/BitUserApi/AcceptFriendRequest/5
        [HttpPut("AcceptFriendRequest/{id}")]
        public async Task<IActionResult> AcceptFriendRequest(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser reciver = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser sender = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            _bitUserRepository.AcceptFriendRequest(sender, reciver);

            return Ok();
        }
        [HttpPut("DeclineFriendRequest/{id}")]
        public async Task<IActionResult> DeclineFriendRequest(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser reciver = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser sender = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            _bitUserRepository.DeclineFriendRequest(sender, reciver);

            return Ok();
        }


        // PUT: api/BitUserApi/RemoveFriend/5
        [HttpPut("RemoveFriend/{id}")]
        public async Task<IActionResult> RemoveFriend(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser reciver = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            await _bitUserRepository.RemoveFriend(sender, reciver);

            return Ok();
        }
        // GET: api/BitUserApi/CheckIfFriends/5
        [HttpGet("CheckIfFriends/{id}")]
        public async Task<ActionResult<bool>> CheckIfFriends(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser reciver = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            return _bitUserRepository.CheckIfFriends(sender, reciver);
        }
        // GET: api/BitUserApi/CheckIfRequestSent/5
        [HttpGet("CheckIfRequestSent/{id}")]
        public async Task<ActionResult<bool>> CheckIfRequestSent(int id)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser reciver = _bitUserRepository.GetBitUserByRegularId(id);
            if (sender == null || reciver == null)
            {
                return NotFound();
            }
            return _bitUserRepository.CheckIfRequestSent(sender, reciver);
        }
        // GET: api/BitUserApi/GetFriends
        [HttpGet("GetFriends")]
        public async Task<ActionResult<IEnumerable<BitUserDTO>>> GetFriends()
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            if (sender == null)
            {
                return NotFound();
            }
            var friends = await _bitUserRepository.GetFriends(sender.Id);
            if (friends == null)
            {
                return NotFound();
            }
            var DtoBitUsers = friends.Select(u => u.ReturnBitUserSearchDTO()).ToList();
            return DtoBitUsers;
        }
        // GET: api/BitUserApi/GetFriendRequests
        [HttpGet("GetFriendRequests")]
        public async Task<ActionResult<IEnumerable<FriendRequest>>> GetFriendRequests()
        {
            string senderId = _userManager.GetUserId(User);
            BitUser sender = _bitUserRepository.GetBitUserByEntityId(senderId);
            if (sender == null)
            {
                return NotFound();
            }
            IEnumerable<FriendRequest> friendRequests = await _bitUserRepository.GetFriendRequests(sender.Id);
            if (friendRequests == null)
            {
                return NotFound();
            }
            friendRequests = friendRequests.Where(fr => fr.Status == "Pending");

            return Ok(friendRequests);
        }
        // PUT: api/BitUserApi/BlockUser/{name}
        [HttpPut("BlockUser/{name}")]
        public async Task<IActionResult> BlockUser(string name)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser viewer = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser personBeingViewed = _bitUserRepository.GetBitUserByName(name);
            await RemoveFriend(personBeingViewed.Id);
            if (viewer == null || personBeingViewed == null)
            {
                return NotFound();
            }
            
            /* sender.BlockedUsers.Add(new BlockedUser
            {
                BlockerId = viewer.id
                BlockedUserId = personBeingViewed.Id
            });
            _bitUserRespository.BlockUser(viewer ,personBeingViewed);
            */
            return Ok();
        }
        // PUT: api/BitUserApi/UnblockUser/{name}
        [HttpPut("UnblockUser/{name}")]
        public async Task<IActionResult> UnblockUser(string name)
        {
            string senderId = _userManager.GetUserId(User);
            BitUser viewer = _bitUserRepository.GetBitUserByEntityId(senderId);
            BitUser personBeingViewed = _bitUserRepository.GetBitUserByName(name);
            if (viewer == null || personBeingViewed == null)
            {
                return NotFound();
            }
            /* BlockedUser blockedUser = viewer.BlockedUsers.FirstOrDefault(bu => bu.BlockedUserId == personBeingViewed.Id);
               if (blockedUser != null)
               {
               viewer.BlockedUsers.Remove(blockedUser);
               _bitUserRepository.AddOrUpdate(viewer);
               }
                */
            return Ok();
        }
    }
}
