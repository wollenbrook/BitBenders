//Controllers/UserAnnouncementsApiControllerTests.cs

using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using BitBracket.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace BitBracket.Tests
{
    public class UserAnnouncementsApiControllerTests
    {
        private Mock<IUserAnnouncementRepository> _mockAnnouncementRepo;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<ITournamentRepository> _mockTournamentRepo;
        private Mock<IBitUserRepository> _mockBitUserRepo;
        private Mock<IEmailService> _mockEmailService;
        private Mock<ILogger<UserAnnouncementsApiController>> _mockLogger;
        private UserAnnouncementsApiController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAnnouncementRepo = new Mock<IUserAnnouncementRepository>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);
            _mockTournamentRepo = new Mock<ITournamentRepository>();
            _mockBitUserRepo = new Mock<IBitUserRepository>();
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<UserAnnouncementsApiController>>();

            _controller = new UserAnnouncementsApiController(
                _mockAnnouncementRepo.Object,
                _mockUserManager.Object,
                _mockTournamentRepo.Object,
                _mockBitUserRepo.Object,
                _mockEmailService.Object,
                _mockLogger.Object
            );

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "user1"),
            }));

            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        [Test]
        public async Task GetTournamentsByOwner_ReturnsNotFound_WhenUserNotFound()
        {
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns((BitUser)null);

            var result = await _controller.GetTournamentsByOwner();

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task GetDraftAnnouncements_ReturnsOkObjectResult_WithAnnouncements()
        {
            var mockAnnouncements = new List<UserAnnouncement>
            {
                new UserAnnouncement { Id = 1, Title = "Draft Announcement" }
            };
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(new BitUser());
            _mockAnnouncementRepo.Setup(x => x.GetByUserIdAndStatus(It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(mockAnnouncements);

            var result = await _controller.GetDraftAnnouncements();

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.AreEqual(mockAnnouncements, okResult.Value);
        }

        [Test]
        public async Task PublishAnnouncement_ReturnsUnauthorized_WhenUserNotFound()
        {
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns((BitUser)null);

            var result = await _controller.PublishAnnouncement(1);

            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

        [Test]
        public async Task PublishAnnouncement_ReturnsNotFound_WhenAnnouncementNotFound()
        {
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(new BitUser { Id = 1 });
            _mockAnnouncementRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserAnnouncement)null);

            var result = await _controller.PublishAnnouncement(1);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task PublishAnnouncement_ReturnsOk_WhenSuccessful()
        {
            var user = new BitUser { Id = 1 };
            var announcement = new UserAnnouncement { Id = 1, Owner = 1, IsDraft = true };

            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(user);
            _mockAnnouncementRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(announcement);
            _mockAnnouncementRepo.Setup(x => x.UpdateAsync(It.IsAny<UserAnnouncement>())).Verifiable();

            var result = await _controller.PublishAnnouncement(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.IsFalse(announcement.IsDraft);
        }
        [Test]
        public async Task UpdateAnnouncement_ReturnsNotFound_WhenAnnouncementNotFound()
        {
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(new BitUser { Id = 1 });
            _mockAnnouncementRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserAnnouncement)null);

            var result = await _controller.UpdateAnnouncement(1, new UserAnnouncement());

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task DeleteAnnouncement_ReturnsNotFound_WhenAnnouncementNotFound()
        {
            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(new BitUser { Id = 1 });
            _mockAnnouncementRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserAnnouncement)null);

            var result = await _controller.DeleteAnnouncement(1);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task DeleteAnnouncement_ReturnsOk_WhenSuccessful()
        {
            var user = new BitUser { Id = 1 };
            var announcement = new UserAnnouncement { Id = 1, Owner = 1 };

            _mockBitUserRepo.Setup(x => x.GetBitUserByEntityId(It.IsAny<string>())).Returns(user);
            _mockAnnouncementRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(announcement);
            _mockAnnouncementRepo.Setup(x => x.DeleteAsync(It.IsAny<int>())).Verifiable();

            var result = await _controller.DeleteAnnouncement(1);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

    }
}
