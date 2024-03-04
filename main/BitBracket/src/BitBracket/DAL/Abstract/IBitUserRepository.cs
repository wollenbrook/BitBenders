using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IBitUserRepository
    {
        public BitUser GetBitUserByEntityId(string id);
        public void UpdateBitUserProfilePictureIfNull(BitUser user);


    }
}

