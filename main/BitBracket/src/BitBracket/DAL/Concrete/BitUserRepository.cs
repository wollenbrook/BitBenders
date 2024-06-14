﻿//DAL/Concrete/BitUserRepository.cs

using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using NuGet.Protocol.Plugins;
using System.Reflection;
using Twilio.TwiML.Fax;

namespace BitBracket.DAL.Concrete
{
    public class BitUserRepository : Repository<BitUser>, IBitUserRepository
    {
        private readonly DbSet<BitUser> _bitUsers;
        private readonly DbSet<FriendRequest> _friendRequests;
        private readonly BitBracketDbContext _context;

        public BitUserRepository(BitBracketDbContext context) : base(context)
        {
            _bitUsers = context.BitUsers;
            _friendRequests = context.FriendRequests;
            _context = context;
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

        public Task SendFriendRequest(BitUser sender, BitUser reciver)
        {

            if (sender == null || reciver == null)
            {
                throw new WebException("User not found");
            }

            FriendRequest friendRequest = new FriendRequest { SenderId = sender.Id, ReceiverId = reciver.Id, Status = "Pending" };

            sender.FriendRequestSenders.Add(friendRequest);
            reciver.FriendRequestReceivers.Add(friendRequest);
            _bitUsers.Update(sender);
            _bitUsers.Update(reciver);

            _friendRequests.Add(friendRequest);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task AcceptFriendRequest(BitUser sender, BitUser reciever)
        {

            if (sender == null || reciever == null)
            {
                throw new WebException("User not found");
            }
            reciever.FriendRequestReceivers.FirstOrDefault(r => r.SenderId == sender.Id && r.ReceiverId == reciever.Id).Status = "Accepted";
            
            sender.FriendRequestSenders.FirstOrDefault(r => r.Sender.Id == sender.Id && r.ReceiverId == reciever.Id).Status = "Accepted";

            sender.FriendUsers.Add(new Friend {UserId = sender.Id, FriendId = reciever.Id });
            reciever.FriendUsers.Add(new Friend { UserId = reciever.Id, FriendId = sender.Id });
            _bitUsers.Update(sender);
            _bitUsers.Update(reciever);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task DeclineFriendRequest(BitUser Sender, BitUser Reciver)
        {

            if (Sender == null || Reciver == null)
            {
                throw new WebException("User not found");
            }

            FriendRequest friendRequest = _friendRequests.FirstOrDefault(fr => fr.SenderId == Sender.Id && fr.ReceiverId == Reciver.Id && fr.Status == "Pending");

            if (friendRequest != null)
            {
                Reciver.FriendRequestReceivers.Remove(friendRequest);
            }
            if (friendRequest != null)
            {
                Sender.FriendRequestSenders.Remove(friendRequest);
            }
            _friendRequests.Remove(friendRequest);
            _bitUsers.Update(Sender);
            _bitUsers.Update(Reciver);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public Task RemoveFriend(BitUser UserPerson, BitUser Removed)
        {

            if (UserPerson == null || Removed == null)
            {
                throw new WebException("User not found");
            }
            Friend friend = UserPerson.FriendUsers.FirstOrDefault(f => f.FriendId == Removed.Id);
            if (friend == null)
            {
                throw new WebException("Friend not found");
            }
            UserPerson.FriendUsers.Remove(friend);
            friend = Removed.FriendUsers.FirstOrDefault(f => f.FriendId == UserPerson.Id);
            if (friend == null)
            {
                throw new WebException("Friend not found");
            }
            Removed.FriendUsers.Remove(friend);

            FriendRequest request = _friendRequests.FirstOrDefault(fr => fr.SenderId == UserPerson.Id && fr.ReceiverId == Removed.Id && fr.Status == "Accepted");
            if (request != null)
            {
                UserPerson.FriendRequestSenders.Remove(request);
                Removed.FriendRequestReceivers.Remove(request);
                _friendRequests.Remove(request);

            }
            else
            {
                request = _friendRequests.FirstOrDefault(fr => fr.SenderId == Removed.Id && fr.ReceiverId == UserPerson.Id && fr.Status == "Accepted");
                if (request != null)
                {
                    UserPerson.FriendRequestReceivers.Remove(request);
                    Removed.FriendRequestSenders.Remove(request);
                    _friendRequests.Remove(request);
                }
            }
            _bitUsers.Update(UserPerson);
            _bitUsers.Update(Removed);
            _context.SaveChanges();
            return Task.CompletedTask;
        }

        public bool CheckIfFriends(BitUser Sender, BitUser Reciver)
        {
            if (Sender == null || Reciver == null)
            {
                throw new Exception("User not found");
            }
            return Sender.FriendUsers.Any(f => f.FriendId == Reciver.Id);

        }
        public bool CheckIfRequestSent(BitUser Sender, BitUser Reciever)
        {
            if (Sender == null || Reciever == null)
            {
                throw new Exception("User not found");
            }
            return Sender.FriendRequestSenders.Any(fr => fr.ReceiverId == Reciever.Id && fr.Status == "Pending");
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

        public Task<IEnumerable<FriendRequest>> GetFriendRequests(int id)
        {
            BitUser user = _bitUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new WebException("User not found");
            }
            return Task.FromResult(user.FriendRequestReceivers.AsEnumerable());
        }
        public Task<IEnumerable<BlockedUser>> GetAllBlockedUsers(int id)
        {
            BitUser user = _bitUsers.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return Task.FromResult(user.BlockedUserBlockeds.AsEnumerable());

        }
        
        public Task BlockUser(BitUser viewer, BitUser personBeingViewed)
        {
            bool areTheyFriends = CheckIfFriends(viewer, personBeingViewed);
            if (areTheyFriends)
            {
                RemoveFriend(viewer, personBeingViewed);
            }
            BlockedUser blockedUser = new BlockedUser { BlockedId = viewer.Id, BlockedUserId = personBeingViewed.Id };
             viewer.BlockedUserBlockeds.Add(blockedUser);
             _bitUsers.Update(viewer);
             _context.SaveChanges(); 
             
            return Task.CompletedTask;
        }
        public Task UnBlockUser(BitUser viewer, BitUser personBeingViewed)
        {
            BlockedUser blockedUser = viewer.BlockedUserBlockeds.FirstOrDefault(b => b.BlockedUserId == personBeingViewed.Id);
            if (blockedUser == null)
            {
                throw new WebException("User not found");
            }
            viewer.BlockedUserBlockeds.Remove(blockedUser);
            _bitUsers.Update(viewer);
            _context.SaveChanges();
            return Task.CompletedTask;
        }
        public IEnumerable<BitUser> GetOptedInUsers()
        {
            return _bitUsers.Where(u => u.OptInConfirmation).ToList();
        }
        public int GetEstimatedSkillLevel(BitUser user)
        {
            if (user.Standings.Count == 0)
            {
                return 0;
            }
            int totalPoints = 0;
            List<int> placementList = [];
            Dictionary<int, int> dictionaryOfPoints = new Dictionary<int, int>()
            {
            {1, 80},
            {2, 70},
            {3, 60},
            {4, 50},
            {5, 40},
            {6, 30},
            {7, 20},
            {8, 10}
            };
            // Calculate placement points

            foreach (Standing standings in user.Standings)
            {
                if (standings.Placement > 8)
                {
                    totalPoints += 5;
                }
                else
                {
                    totalPoints += dictionaryOfPoints[standings.Placement];
                }
                placementList.Add(standings.Placement);
            }
            totalPoints = totalPoints / placementList.Count;
            placementList.Sort();
            // Calculate bonus points based on consistency
            if (placementList.Take(3).All(p => p - placementList.Min() <= 2))
            {
                totalPoints += 10; 
            }
            int skillLevel = totalPoints / 10;  // Adjust divisor as needed
            float skillLevelFloat = totalPoints / 10;
            skillLevel =  (int)Math.Floor(skillLevelFloat);
            if (skillLevel > 8)
            {
                skillLevel = 8;
            }
            return skillLevel;
        }

        public async Task<BitUser> GetUserByIdAsync(int id)
        {
            return await _bitUsers.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateUserAsync(BitUser user)
        {
            _bitUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }

}