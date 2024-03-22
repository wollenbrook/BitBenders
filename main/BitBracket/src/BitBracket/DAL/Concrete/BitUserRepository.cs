using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using BitBracket.DAL.Concrete;
using HW6.DAL.Concrete;


namespace BitBracket.DAL.Concrete
{
    public class BitUserRepository : Repository<BitUser>, IBitUserRepository
    {
        private DbSet<BitUser> _bitUsers;
        public BitUserRepository(BitBracketDbContext context) : base(context)
        {
            _bitUsers = context.BitUser;
        }

        public Task DeleteBitUser(BitUser user)
        {
            if (user == null)
            {
                throw new WebException("User not found");
            }
            _bitUsers.Remove(user);
            return Task.CompletedTask;
        }

        public BitUser GetBitUserByEntityId(string id)
        {
            return _bitUsers.FirstOrDefault(u => u.AspnetIdentityId == id);
        }

        public BitUser GetBitUserByRegularId(int id)
        {

            return _bitUsers.FirstOrDefault(u => u.Id == id);
        }
        public BitUser GetBitUserByName(string username)
        {
            return _bitUsers.FirstOrDefault(u => u.Username == username);
        }


        public async Task UpdateBitUserProfilePictureIfNull(BitUser user)
        {
            using (var client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync("https://bitbracketimagestorage.blob.core.windows.net/images/Blank_Profile.png");
                user.ProfilePicture = imageBytes;
            }
            _bitUsers.Update(user);

        }

        public Task UpdateUserName(BitUser user)
        {
            _bitUsers.Update(user);
            return Task.CompletedTask;
            
        }

    }

}