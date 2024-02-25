using BitBracket.Controllers;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBracket_NUnit_Tests
{
    public class AnnouncementsTests
    {
        private AnnouncementsApiController _controller;
        private Mock<IAnnouncementRepository> _mockAnnouncementRepo;
        private Mock<IEmailService> _mockEmailService;
        private Mock<UserManager<IdentityUser>> _mockUserManager;

        [SetUp]
        public void Setup()
        {
            _mockAnnouncementRepo = new Mock<IAnnouncementRepository>();
            _mockEmailService = new Mock<IEmailService>();

            var store = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            _controller = new AnnouncementsApiController(_mockAnnouncementRepo.Object, _mockEmailService.Object, _mockUserManager.Object);

            var users = new List<IdentityUser>
            {
                new IdentityUser { Email = "user1@example.com", EmailConfirmed = true },
                new IdentityUser { Email = "user2@example.com", EmailConfirmed = false }
            }.AsQueryable();

            _mockUserManager.Setup(x => x.Users).Returns(users);
        }

        // Announcement Tests

        [Test]
        public async Task Create_ValidAnnouncement_ReturnsOk()
        {
            var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };
            _mockAnnouncementRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

            var result = await _controller.Create(newAnnouncement, "YourAdminKey");

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidAdminKey_ReturnsUnauthorized()
        {
            var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };

            var result = await _controller.Create(newAnnouncement, "WrongKey");

            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

        [Test]
        public async Task GetAll_ReturnsAllAnnouncements()
        {
            var allAnnouncements = new List<Announcement> { new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" } };
            _mockAnnouncementRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allAnnouncements);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedAnnouncements = okResult.Value as IEnumerable<Announcement>;
            Assert.AreEqual(1, (returnedAnnouncements as List<Announcement>).Count);
        }

        // Email Sending Tests

        [Test]
        public async Task CreateAnnouncement_SendsEmailToConfirmedUsers()
        {
            var announcement = new Announcement { Title = "New Event", Description = "Event Description", Author = "Organizer" };
            _mockAnnouncementRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

            await _controller.Create(announcement, "YourAdminKey");

            _mockEmailService.Verify(es => es.SendEmailAsync(
                It.Is<string>(email => email == "user1@example.com"), 
                It.IsAny<string>(), 
                It.IsAny<object>()), Times.Once);

            _mockEmailService.Verify(es => es.SendEmailAsync(
                It.Is<string>(email => email == "user2@example.com"), 
                It.IsAny<string>(), 
                It.IsAny<object>()), Times.Never);
        }
    }
}
