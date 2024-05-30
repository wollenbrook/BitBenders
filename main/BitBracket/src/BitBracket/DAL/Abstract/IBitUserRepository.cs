﻿using BitBracket.Models;
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

        public Task SendFriendRequest(BitUser sender, BitUser reciver);
        public Task AcceptFriendRequest(BitUser sender, BitUser reciver);
        public Task DeclineFriendRequest(BitUser sender, BitUser reciver);
        public Task RemoveFriend(BitUser sender, BitUser reciver);
        public bool CheckIfFriends(BitUser sender, BitUser reciver);
        public bool CheckIfRequestSent(BitUser sender, BitUser reciver);
        public Task<IEnumerable<BitUser>> GetFriends(int id);
        IEnumerable<BitUser> GetOptedInUsers();
        public Task<IEnumerable<BlockedUser>> GetAllBlockedUsers(int id);
        public Task BlockUser(BitUser viewer, BitUser personBeingViewed);
        public Task UnBlockUser(BitUser viewer, BitUser personBeingViewed);
        public Task<IEnumerable<FriendRequest>> GetFriendRequests(int id);
        Task<BitUser> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(BitUser user);
        public int GetEstimatedSkillLevel(BitUser user);
    }
}
