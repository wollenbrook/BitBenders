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


namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitUserApiController : ControllerBase
    {
        private readonly BitBracketDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IBitUserRepository _bitUserRepository;


        public BitUserApiController(BitBracketDbContext context, UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository)
        {
            _context = context;
            _userManager = userManager;
            _bitUserRepository = bitUserRepository;
        }

        // GET: api/BitUserApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BitUser>>> GetBitUser()
        {
            return await _context.BitUser.ToListAsync();
        }

        // GET: api/BitUserApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BitUser>> GetBitUser(int id)
        {
            var bitUser = await _context.BitUser.FindAsync(id);

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
            BitUser bitUser = _context.BitUser.Where(u => u.AspnetIdentityId == id).FirstOrDefault();
            bitUser.Bio = bio; 
            _context.Entry(bitUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("TagChange/{tag}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateBitUserTag(string tag)
        {
            string id = _userManager.GetUserId(User);
            BitUser bitUser = _context.BitUser.Where(u => u.AspnetIdentityId == id).FirstOrDefault();
            bitUser.Tag = tag;
            _context.Entry(bitUser).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }

        // PUT: api/BitUserApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutBitUser(int id, BitUser bitUser)
        {
            if (id != bitUser.Id)
            {
                return BadRequest();
            }

            _context.Entry(bitUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BitUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        */

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
                _context.Entry(bitUser).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok();


            }

            // Handle the case when no file is uploaded
            return BadRequest();
        }

        /*[HttpPost]
        public async Task<ActionResult<BitUser>> PostBitUser(BitUser bitUser)
        {
            _context.BitUser.Add(bitUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBitUser", new { id = bitUser.Id }, bitUser);
        }
        */

        // DELETE: api/BitUserApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBitUser(int id)
        {
            var bitUser = await _context.BitUser.FindAsync(id);
            if (bitUser == null)
            {
                return NotFound();
            }

            _context.BitUser.Remove(bitUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BitUserExists(int id)
        {
            return _context.BitUser.Any(e => e.Id == id);
        }
    }
}
