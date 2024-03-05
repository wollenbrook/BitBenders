using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BitBracket.DAL.Concrete
{
    public class BitUserRepository : IBitUserRepository
    {
        private readonly BitBracketDbContext _context;

        public BitUserRepository(BitBracketDbContext context)
        {
            _context = context;
        }

        public BitUser GetBitUserByEntityId(string id)
        {
            return _context.Set<BitUser>().FirstOrDefault(u => u.AspnetIdentityId == id);
        }

        public async Task UpdateBitUserProfilePictureIfNull(BitUser user)
        {
            using (var client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync("https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png");
                user.ProfilePicture = imageBytes;
            }
            _context.Update(user);
            _context.SaveChanges();

        }
    }
    
}


