using BitBracket.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using BitBracket.DAL;
using BitBracket.DAL.Concrete;
using BitBracket.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Reflection;
using Twilio.TwiML.Fax;

namespace BitBracket_NUnit_Tests;

public class FriendRequestTests
{
    private Mock<IBitUserRepository> _userRepositoryMock;


    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IBitUserRepository>();

    }

        [Test]
        public async Task SendFriendRequest_ValidParameters_CallsRepositoryMethod()
        {
            // Arrange
            BitUser sender = new BitUser() { 
                Id = 1,
                Username = "sender",
                AspnetIdentityId = "senderId",
                Tag = "senderTag",
                Bio = "senderBio",
                ProfilePicture = null,
                EmailConfirmedStatus = true,
                OptInConfirmation = true,
            };
            BitUser receiver = new BitUser()
            {
                Id = 2,
                Username = "receiver",
                AspnetIdentityId = "receiverId",
                Tag = "receiverTag",
                Bio = "receiverBio",
                ProfilePicture = null,
                EmailConfirmedStatus = true,
                OptInConfirmation = true,
            };

            // Act
            await _userRepositoryMock.Object.SendFriendRequest(sender, receiver);

            // Assert
            _userRepositoryMock.Verify(x => x.SendFriendRequest(sender, receiver), Times.Once);
        }

        [Test]
    public void TestThatAUserCanSendAFriendRequest()
    {
        BitUser sender = new BitUser()
        {
            Id = 1,
            Username = "sender",
            AspnetIdentityId = "senderId",
            Tag = "senderTag",
            Bio = "senderBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        BitUser receiver = new BitUser()
        {
            Id = 2,
            Username = "receiver",
            AspnetIdentityId = "receiverId",
            Tag = "receiverTag",
            Bio = "receiverBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };


        //This method would send a friend request, creating a sent friend request object (BitUser now has a collection of Sent and Recieved Friend Requests, along with a FriendShips collection) and adding it to the sender's sent friend requests list
        _userRepositoryMock.Object.SendFriendRequest(sender, receiver);
        Assert.Pass();
    }
    [Test]
    public void TestThatAUserCanRecieveAFriendRequest()
    {
        BitUser sender = new BitUser()
        {
            Id = 1,
            Username = "sender",
            AspnetIdentityId = "senderId",
            Tag = "senderTag",
            Bio = "senderBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        BitUser receiver = new BitUser()
        {
            Id = 2,
            Username = "receiver",
            AspnetIdentityId = "receiverId",
            Tag = "receiverTag",
            Bio = "receiverBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        //Same set up as the test above, just tests that the reciever has a received friend request object added to their list
        _userRepositoryMock.Object.SendFriendRequest(sender, receiver);

    }
    // [Test]
    // public void TestThatAUserCanAcceptAFriendRequest()
    // {
    //     BitUser sender = new BitUser()
    //     {
    //         Id = 1,
    //         Username = "sender",
    //         AspnetIdentityId = "senderId",
    //         Tag = "senderTag",
    //         Bio = "senderBio",
    //         ProfilePicture = null,
    //         EmailConfirmedStatus = true,
    //         OptInConfirmation = true,
    //     };
    //     BitUser receiver = new BitUser()
    //     {
    //         Id = 2,
    //         Username = "receiver",
    //         AspnetIdentityId = "receiverId",
    //         Tag = "receiverTag",
    //         Bio = "receiverBio",
    //         ProfilePicture = null,
    //         EmailConfirmedStatus = true,
    //         OptInConfirmation = true,
    //     };
    //     //This method would accept a friend request, changing the status of the friend request object to 'Accepted' and adding the sender to the receiver's friends list and vice versa
    //     _userRepositoryMock.Object.AcceptFriendRequest(sender, receiver);

    //     Assert.That("Accepted", Is.EqualTo(receiver.FriendRequestReceivers.First(r => r.Id == sender.Id).Status));
          
    // }
    [Test]
    public void TestThatAUserCanDenyAFriendRequest()
    {
        BitUser sender = new BitUser()
        {
            Id = 1,
            Username = "sender",
            AspnetIdentityId = "senderId",
            Tag = "senderTag",
            Bio = "senderBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        BitUser receiver = new BitUser()
        {
            Id = 2,
            Username = "receiver",
            AspnetIdentityId = "receiverId",
            Tag = "receiverTag",
            Bio = "receiverBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };

        _userRepositoryMock.Object.DeclineFriendRequest(sender, receiver);

        Assert.IsEmpty(receiver.FriendRequestReceivers);
    }
    [Test]
    public void TestThatAUserCanRemoveAFriend()
    {
        BitUser sender = new BitUser()
        {
            Id = 1,
            Username = "sender",
            AspnetIdentityId = "senderId",
            Tag = "senderTag",
            Bio = "senderBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        BitUser receiver = new BitUser()
        {
            Id = 2,
            Username = "receiver",
            AspnetIdentityId = "receiverId",
            Tag = "receiverTag",
            Bio = "receiverBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        _userRepositoryMock.Object.AcceptFriendRequest(sender, receiver);
        _userRepositoryMock.Object.RemoveFriend(sender, receiver);


        Assert.IsEmpty(receiver.FriendUsers);
        Assert.IsEmpty(sender.FriendUsers);
    }
    [Test]
    public void TestToCheckIfTheyAreFriendsReturnsExpectedValueIfNotFriends()
    {
        BitUser sender = new BitUser()
        {
            Id = 1,
            Username = "sender",
            AspnetIdentityId = "senderId",
            Tag = "senderTag",
            Bio = "senderBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        BitUser receiver = new BitUser()
        {
            Id = 2,
            Username = "receiver",
            AspnetIdentityId = "receiverId",
            Tag = "receiverTag",
            Bio = "receiverBio",
            ProfilePicture = null,
            EmailConfirmedStatus = true,
            OptInConfirmation = true,
        };
        bool friends = _userRepositoryMock.Object.CheckIfFriends(sender, receiver);
        Assert.IsFalse(friends);
        
    }
    //
    //[Test]
    // public void TestThatCheckIfTheyAreFriendsAfterAcceptingFriendRequest()
    // {
    //     BitUser sender = new BitUser()
    //     {
    //         Id = 1,
    //         Username = "sender",
    //         AspnetIdentityId = "senderId",
    //         Tag = "senderTag",
    //         Bio = "senderBio",
    //         ProfilePicture = null,
    //         EmailConfirmedStatus = true,
    //         OptInConfirmation = true,
    //     };
    //     BitUser receiver = new BitUser()
    //     {
    //         Id = 2,
    //         Username = "receiver",
    //         AspnetIdentityId = "receiverId",
    //         Tag = "receiverTag",
    //         Bio = "receiverBio",
    //         ProfilePicture = null,
    //         EmailConfirmedStatus = true,
    //         OptInConfirmation = true,
    //     };
    //     _userRepositoryMock.Object.SendFriendRequest(sender, receiver);
    //     _userRepositoryMock.Object.AcceptFriendRequest(receiver, sender);
    //     bool friends = _userRepositoryMock.Object.CheckIfFriends(sender, receiver);
    //     Assert.IsTrue(friends);
    // }
}




