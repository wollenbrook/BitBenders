// using BitBracket.Controllers;
// using BitBracket.DAL.Abstract;
// using BitBracket.Models;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Moq;
// using NUnit.Framework;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace BitBracket_NUnit_Tests
// {
//     public class AnnouncementsTests
//     {
//         private AnnouncementsApiController _controller;
//         private Mock<IAnnouncementRepository> _mockAnnouncementRepo;
//         private Mock<IEmailService> _mockEmailService;
//         private Mock<ISmsService> _mockSmsService;
//         private Mock<UserManager<IdentityUser>> _mockUserManager;

//         [SetUp]
//         public void Setup()
//         {
//             _mockAnnouncementRepo = new Mock<IAnnouncementRepository>();
//             _mockEmailService = new Mock<IEmailService>();
//             _mockSmsService = new Mock<ISmsService>();

//             var userStoreMock = new Mock<IUserStore<IdentityUser>>();
//             _mockUserManager = new Mock<UserManager<IdentityUser>>(
//                 userStoreMock.Object, null, null, null, null, null, null, null, null);

//             _controller = new AnnouncementsApiController(
//                 _mockAnnouncementRepo.Object, 
//                 _mockEmailService.Object, 
//                 _mockSmsService.Object, 
//                 _mockUserManager.Object);

//             var users = new List<IdentityUser>
//             {
//                 new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com", EmailConfirmed = true, PhoneNumber = "+1234567890", PhoneNumberConfirmed = true },
//                 new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com", EmailConfirmed = false, PhoneNumber = "+0987654321", PhoneNumberConfirmed = false }, // This user won't receive anything
//                 new IdentityUser { UserName = "user3@example.com", Email = "user3@example.com", EmailConfirmed = true } // This user only has an email
//             }.AsQueryable();

//             _mockUserManager.Setup(x => x.Users).Returns(users);
//         }

//         // Announcement Tests

//         [Test]
//         public async Task Create_ValidAnnouncement_ReturnsOk()
//         {
//             var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };
//             _mockAnnouncementRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

//             var result = await _controller.Create(newAnnouncement, "YourAdminKey");

//             Assert.IsInstanceOf<OkObjectResult>(result);
//         }

//         [Test]
//         public async Task Create_InvalidAdminKey_ReturnsUnauthorized()
//         {
//             var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };

//             var result = await _controller.Create(newAnnouncement, "WrongKey");

//             Assert.IsInstanceOf<UnauthorizedResult>(result);
//         }

//         [Test]
//         public async Task GetAll_ReturnsAllAnnouncements()
//         {
//             var allAnnouncements = new List<Announcement> { new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" } };
//             _mockAnnouncementRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allAnnouncements);

//             var result = await _controller.GetAll();

//             var okResult = result as OkObjectResult;
//             Assert.IsNotNull(okResult);
//             var returnedAnnouncements = okResult.Value as IEnumerable<Announcement>;
//             Assert.AreEqual(1, (returnedAnnouncements as List<Announcement>).Count);
//         }

//         // Email Sending Tests

//         [Test]
//         public async Task CreateAnnouncement_SendsEmailToConfirmedUsers()
//         {
//             var announcement = new Announcement { Title = "New Event", Description = "Event Description", Author = "Organizer" };
//             _mockAnnouncementRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

//             await _controller.Create(announcement, "YourAdminKey");

//             _mockEmailService.Verify(es => es.SendEmailAsync(
//                 It.Is<string>(email => email == "user1@example.com"), 
//                 It.IsAny<string>(), 
//                 It.IsAny<object>()), Times.Once);

//             _mockEmailService.Verify(es => es.SendEmailAsync(
//                 It.Is<string>(email => email == "user2@example.com"), 
//                 It.IsAny<string>(), 
//                 It.IsAny<object>()), Times.Never);
//         }

//        //SMS/Email Sending Tests

//         // [Test]
//         // public async Task CreateAnnouncement_SendsEmailAndSmsToEligibleUsers()
//         // {
//         //     var announcement = new Announcement { Title = "New Event", Description = "Event Description", Author = "Organizer" };
//         //     _mockAnnouncementRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

//         //     await _controller.Create(announcement, "YourAdminKey");

//         //     // Verify email sent to users with confirmed emails
//         //     _mockEmailService.Verify(es => es.SendEmailAsync(
//         //         It.IsAny<string>(), 
//         //         It.Is<string>(subject => subject == "New Announcement published on the BitBracketApp"), 
//         //         It.IsAny<object>()), 
//         //         Times.Exactly(2)); // Assuming two users have confirmed emails

//         //     // Verify SMS sent to users with confirmed phone numbers
//         //     _mockSmsService.Verify(sms => sms.SendSmsAsync(
//         //         It.Is<string>(phone => phone == "+1234567890"),
//         //         It.IsAny<string>()), 
//         //         Times.Once); // Only one user has a confirmed phone number
//         // }
//     }
// }