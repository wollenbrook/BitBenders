using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IBitUserRepository : IRepository<BitUser>
    {
        public BitUser GetBitUserByEntityId(string id);
        public Task UpdateBitUserProfilePictureIfNull(BitUser user);
        public Task DeleteBitUser(BitUser user);
        public Task UpdateUserName(BitUser user);
        public BitUser GetBitUserByRegularId(int id);
        public List<BitUser> GetBitUserByName(string username);

    }
}
