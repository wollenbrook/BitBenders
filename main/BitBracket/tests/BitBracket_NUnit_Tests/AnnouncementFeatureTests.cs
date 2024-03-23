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
    public class AnnouncementsSprint4Tests
    {
        private AnnouncementsApiController _controller;
        private Mock<IAnnouncementRepository> _mockAnnouncementRepo;
        private Mock<IEmailService> _mockEmailService;
        private Mock<ISmsService> _mockSmsService;
        private Mock<UserManager<IdentityUser>> _mockUserManager;

        [SetUp]
        public void Setup()
        {
            _mockAnnouncementRepo = new Mock<IAnnouncementRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockSmsService = new Mock<ISmsService>();

            var userStoreMock = new Mock<IUserStore<IdentityUser>>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            _controller = new AnnouncementsApiController(
                _mockAnnouncementRepo.Object, 
                _mockEmailService.Object, 
                _mockSmsService.Object, 
                _mockUserManager.Object);

            var users = new List<IdentityUser>
            {
                new IdentityUser { UserName = "user1@example.com", Email = "user1@example.com", EmailConfirmed = true, PhoneNumber = "+1234567890", PhoneNumberConfirmed = true },
                new IdentityUser { UserName = "user2@example.com", Email = "user2@example.com", EmailConfirmed = false, PhoneNumber = "+0987654321", PhoneNumberConfirmed = false }, // This user won't receive anything
                new IdentityUser { UserName = "user3@example.com", Email = "user3@example.com", EmailConfirmed = true } // This user only has an email
            }.AsQueryable();

            _mockUserManager.Setup(x => x.Users).Returns(users);
        }

        [Test]
        public async Task CreateAnnouncement_WhenUserNotLoggedIn_ReturnsUnauthorized()
        {
            // TODO: Implement test logic
        }

        [Test]
        public async Task CreateAnnouncement_WhenUserHasNoTournaments_ReturnsBadRequest()
        {
            // TODO: Implement test logic
        }

        [Test]
        public async Task CreateAnnouncement_WhenDataIsValid_AndUserIsOrganizer_SavesAnnouncement()
        {
            // TODO: Implement test logic
        }

        [Test]
        public async Task OptInForEmailNotifications_UpdatesUserPreference_Successfully()
        {
            // TODO: Implement test logic
        }

        [Test]
        public async Task OptOutOfEmailNotifications_UpdatesUserPreference_Successfully()
        {
            // TODO: Implement test logic
        }

        [Test]
        public async Task AnnouncementEmail_IsSentToUsersWhoOptedIn()
        {
            // TODO: Implement test logic
        }
    }
}
