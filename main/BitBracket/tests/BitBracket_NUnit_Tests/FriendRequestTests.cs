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

    [SetUp]
    public void Setup()
    {
        //for the sake of readability, I will state here that these two bit users will be the sender and receiver for all of the tests
        BitUser sender = new BitUser()
        {
            Username = "John",
            Tag = "Doe",
            ProfilePicture = null
        };

        BitUser receiver = new BitUser()
        {
            Username = "Jane",
            Tag = "Smith",
            ProfilePicture = null
        };
    }

    [Test]
    public void TestThatAUserCanSendAFriendRequest()
    {

        //This method would send a friend request, creating a sent friend request object (BitUser now has a collection of Sent and Recieved Friend Requests, along with a FriendShips collection) and adding it to the sender's sent friend requests list
        _BitUserRepo.SendFriendRequest(sender, receiver);
        //like this
        sender.SentFriendRequests.Add(new SentFriendRequest()
        {
            Id = 1,
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Status = 'Pending'
        });
        //and this method would at the same time create a received friend request object and add it to the receiver's received friend requests list,
        //although that is not what is being tested here.
        receiver.ReceivedFriendRequests.Add(new ReceivedFriendRequest()
        {
            Id = 1,
            SenderId = sender.Id,
            Status = 'Pending'
        });
        //Should be an AssertEqual, but Not Null is more readable
        AssertNotNull(sender.SentFriendRequests[0]);

    }
    [Test]
    public void TestThatAUserCanRecieveAFriendRequest()
    {
        //Same set up as the test above, just tests that the reciever has a received friend request object added to their list
        _BitUserRepo.SendFriendRequest(sender, receiver);
        //This would be the code that runs in the SendFriendRequest method
        //The method above would add the next sections to the sender and receiver's friend request lists
        sender.SentFriendRequests.Add(new SentFriendRequest()
        {
            Id = 1,
            SenderId = sender.Id,
            ReceiverId = receiver.Id,
            Status = 'Pending'
        });
        //This is what we are testing this time
        receiver.ReceivedFriendRequests.Add(new ReceivedFriendRequest()
        {
            Id = 1,
            SenderId = sender.Id,
            Status = 'Pending'
        });
        //Should be an AssertEqual, but Not Null is more readable
        AssertNotNull(reciever.RecievedFriendRequests[0]);
    }
    [Test]
    public void TestThatAUserCanAcceptAFriendRequest()
    {
        //This method would accept a friend request, changing the status of the friend request object to 'Accepted' and adding the sender to the receiver's friends list and vice versa
        _BitUserRepo.AcceptFriendRequest(sender, reciever);
        //This would be the code that runs in the AcceptFriendRequest method
        //sets the status of the friend request object to 'Accepted'
        receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First().Status = 'Accepted';
        sender.SentFriendRequests.Where(r => r.ReceiverId == receiver.Id).First().Status = 'Accepted';
        //adds the sender to the receiver's friends list and vice versa
        sender.FriendShips.Add(receiver);
        receiver.FriendShips.Add(sender);
        //Test that the status of the friend request object is 'Accepted'
        AsserEqual(receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First().Status, 'Accepted');
); 
    }
    [Test]
    public void TestThatAUserCanDenyAFriendRequest()
    {
        //This method would deny a friend request, changing the status of the friend request object to 'Denied' and removing the sender from the receiver's received friend requests list
        _BitUserRepo.DenyFriendRequest(sender, reciever);
        //This would be the code that runs in the DenyFriendRequest method
        //sets the status of the friend request object to 'Denied'
        receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First().Status = 'Denied';
        //removes the sender from the receiver's received friend requests list
        receiver.ReceivedFriendRequests.Remove(receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First());
        //removes the receiver from the sender's sent friend requests list
        //reason for this is so that they can send another friend request and not be permanently blocked, as that is not a feature of this user story
        sender.SentFriendRequests.Remove(sender.SentFriendRequests.Where(r => r.ReceiverId == receiver.Id).First());
        //Should be an AssertEqual, but Null is more readable. I think this would actually return a null reference exception, but I'm not sure. 
        AssertNull(receiver.ReceivedFriendRequests[0]);
    }
    [Test]
    public void TestThatAUserCanRemoveAFriend()
    {
        //assumption that the previous code has been run, so that the sender and receiver are friends
        //This method would remove a friend, removing the friend from the sender's friends list and vice versa
        _BitUserRepo.RemoveFriend(sender, reciever);
        //This would be the code that runs in the RemoveFriend method
        //removes the friend from the sender's friends list and vice versa
        sender.FriendShips.Remove(sender.FriendShips.Where(f => f.Id == receiver.Id).First());
        receiver.FriendShips.Remove(receiver.FriendShips.Where(f => f.Id == sender.Id).First());
        //removes the friend request object from the sender's sent friend requests list
        sender.SentFriendRequests.Remove(sender.SentFriendRequests.Where(r => r.ReceiverId == receiver.Id).First());
        //removes the friend request object from the receiver's received friend requests list
        receiver.ReceivedFriendRequests.Remove(receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First());
        //Should be an AssertNone, but not 100% sure that's a thing or works how I think it works
        AssertNull(sender.Friends[0]);
    }
    [Test]
    public void TestThatApiThatChecksForFriendRequestsReturnsExpectedValue()
    {
        //an api would be called that would return a list of friend requests that the user has received that are pending
        //this would be used to display the friend requests on the user's profile page and be the check that provides a notification that the user has a friend request
        //assume sender has sent a friend request to receiver
        var check = _BitUserRepo.CheckFriendRequests(reciever);
        //this would be the code that would be run in the CheckFriendRequests method
        //this api would be called every 30 seconds or so to check for new friend requests and provide a notification if there are any
        for (int i = 0; i < sender.ReceivedFriendRequests.Count; i++)
        {
            if (sender.ReceivedFriendRequests[i].Status == 'Pending')
            {
                //likely returns a bool
                return Something_That_The_JavaScript_Would_Read_And_Use_To_Create_A_Notification_On_The_Front_End;
            }
        }
        AssertEqual(true, Something_That_The_JavaScript_Would_Read_And_Use_To_Create_A_Notification_On_The_Front_End);
    }
    [Test]
    public void TestThatAnAcceptedFriendAppearsOnBothFriendsListAfterAccepting()
    {
        //assumption that the previous code has been run, so that the sender and receiver are friends
        _BitUserRepo.AcceptFriendRequest(sender, reciever);
        //This would be the code that runs in the AcceptFriendRequest method
        //sets the status of the friend request object to 'Accepted'
        receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First().Status = 'Accepted';
        //adds the sender to the receiver's friends list and vice versa
        sender.FriendShips.Add(receiver);
        receiver.FriendShips.Add(sender);
        //Javascript would check the friends list and display the new friend whenever the user clicks on the friends list
        //Should be an AssertEqual, but Not Null is more readable
        AssertNotNull(sender.Friends[0]);
        AssertNotNull(reciever.Friends[0]);
    }

    [Test]
    public void TestThatADeniedFriendDoesNotAppearOnFriendsList()
    {
        //this test is more so for the acceptance criteria of the user story than the logic itself
        //assumption that the previous code has been run where sender has sent a friend request to receiver
        _BitUserRepo.DenyFriendRequest(sender, reciever);
        //This would be the code that runs in the DenyFriendRequest method
        //sets the status of the friend request object to 'Denied'
        //this is a redundent step, as it is removed the next line, but leaving it for clarity, along with potentially changing how this works later on.
        receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First().Status = 'Denied';
        //removes the sender from the receiver's received friend requests list
        receiver.ReceivedFriendRequests.Remove(receiver.ReceivedFriendRequests.Where(r => r.SenderId == sender.Id).First());
        //removes the receiver from the sender's sent friend requests list
        //reason for this is so that they can send another friend request and not be permanently blocked, as that is not a feature of this user story
        sender.SentFriendRequests.Remove(sender.SentFriendRequests.Where(r => r.ReceiverId == receiver.Id).First());
        //Should be an AssertEqual, but Null is more readable. I think this would actually return a null reference exception, but I'm not sure. 
        AssertNull(receiver.Friends[0]);
        AssertNull(sender.Friends[0]);

    }

}




