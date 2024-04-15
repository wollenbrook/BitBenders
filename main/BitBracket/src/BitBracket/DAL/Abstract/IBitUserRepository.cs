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
        public BitUser GetBitUserByName(string username);

        public Task SendFriendRequest(int sender, int reciver);
        public Task AcceptFriendRequest(int sender, int reciver);
        public Task DeclineFriendRequest(int sender, int reciver);
        public Task RemoveFriend(int sender, int reciver);
        public bool CheckIfFriends(int sender, int reciver);
        public Task<IEnumerable<BitUser>> GetFriends(int id);
        public Task<IEnumerable<RecievedFriendRequest>> GetFriendRequests(int id);
        IEnumerable<BitUser> GetOptedInUsers();
    }
}
