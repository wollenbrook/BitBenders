using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IBitUserRepository
    {
        public BitUser GetBitUserByEntityId(string id);
        public Task UpdateBitUserProfilePictureIfNull(BitUser user);
        public Task DeleteBitUser(BitUser user);
        public Task UpdateUserName(BitUser user);


    }
}
