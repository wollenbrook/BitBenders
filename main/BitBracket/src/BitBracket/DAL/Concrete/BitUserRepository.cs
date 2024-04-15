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
            _bitUsers = context.BitUsers;
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

        public Task SendFriendRequest(int sender, int reciver)
        {
            BitUser Sender = _bitUsers.FirstOrDefault(u => u.Id == sender);
            BitUser Reciver = _bitUsers.FirstOrDefault(u => u.Id == reciver);
            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }
            Sender.SentFriendRequestReceivers.Add(new SentFriendRequest { SenderId = Sender.Id, ReceiverId = Reciver.Id, Status = "Pending"});
            Reciver.RecievedFriendRequests.Add(new RecievedFriendRequest { SenderId = Sender.Id, Status = "Pending" });
            _bitUsers.Update(Sender);
            _bitUsers.Update(Reciver);
            return Task.CompletedTask;
        }

        public Task AcceptFriendRequest(int sender, int reciver)
        {
            BitUser Sender = _bitUsers.FirstOrDefault(u => u.Id == sender);
            BitUser Reciver = _bitUsers.FirstOrDefault(u => u.Id == reciver);
            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }
            RecievedFriendRequest request = Reciver.RecievedFriendRequests.FirstOrDefault(r => r.SenderId == sender);
            if (request == null)
            {
                throw new WebException("Request not found");
            }
            request.Status = "Accepted";
            SentFriendRequest sentRequest = Sender.SentFriendRequestSenders.FirstOrDefault(r => r.ReceiverId == reciver);
            if (sentRequest == null)
            {
                throw new WebException("Request not found");
            }
            sentRequest.Status = "Accepted";
            Sender.FriendUsers.Add(new Friend { FriendId = Reciver.Id });
            Reciver.FriendUsers.Add(new Friend { FriendId = Sender.Id });
            _bitUsers.Update(Sender);
            _bitUsers.Update(Reciver);
            return Task.CompletedTask;
        }

        public Task DeclineFriendRequest(int sender, int reciver)
        {
            BitUser Sender = _bitUsers.FirstOrDefault(u => u.Id == sender);
            BitUser Reciver = _bitUsers.FirstOrDefault(u => u.Id == reciver);
            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }
            RecievedFriendRequest request = Reciver.RecievedFriendRequests.FirstOrDefault(r => r.SenderId == sender);
            if (request == null)
            {
                throw new WebException("Request not found");
            }
            request.Status = "Declined";
            SentFriendRequest sentRequest = Sender.SentFriendRequestSenders.FirstOrDefault(r => r.ReceiverId == reciver);
            if (sentRequest == null)
            {
                throw new WebException("Request not found");
            }
            sentRequest.Status = "Declined";
            _bitUsers.Update(Sender);
            _bitUsers.Update(Reciver);
            return Task.CompletedTask;
        }

        public Task RemoveFriend(int sender, int reciver)
        {
            BitUser Sender = _bitUsers.FirstOrDefault(u => u.Id == sender);
            BitUser Reciver = _bitUsers.FirstOrDefault(u => u.Id == reciver);
            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }
            Friend friend = Sender.FriendUsers.FirstOrDefault(f => f.FriendId == reciver);
            if (friend == null)
            {
                throw new WebException("Friend not found");
            }
            Sender.FriendUsers.Remove(friend);
            friend = Reciver.FriendUsers.FirstOrDefault(f => f.FriendId == sender);
            if (friend == null)
            {
                throw new WebException("Friend not found");
            }
            Reciver.FriendUsers.Remove(friend);

            _bitUsers.Update(Sender);
            _bitUsers.Update(Reciver);
            return Task.CompletedTask;
        }

        public bool CheckIfFriends(int sender, int reciver)
        {
            BitUser Sender = _bitUsers.FirstOrDefault(u => u.Id == sender);
            BitUser Reciver = _bitUsers.FirstOrDefault(u => u.Id == reciver);
            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }
            return Sender.FriendUsers.Any(f => f.FriendId == reciver);

        }

        public Task<IEnumerable<BitUser>> GetFriends(int id)
        {
            BitUser user = _bitUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new WebException("User not found");
            }
            List<BitUser> friends = new List<BitUser>();
            foreach (Friend friend in user.FriendUsers)
            {
                friends.Add(_bitUsers.FirstOrDefault(u => u.Id == friend.FriendId));
            }
            return Task.FromResult(friends.AsEnumerable());
        }

        public Task<IEnumerable<RecievedFriendRequest>> GetFriendRequests(int id)
        {
            BitUser user = _bitUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new WebException("User not found");
            }
            return Task.FromResult(user.RecievedFriendRequests.AsEnumerable());
        }
        public IEnumerable<BitUser> GetOptedInUsers()
        {
            return _bitUsers.Where(u => u.OptInConfirmation).ToList();
        }
    }

}