using Moq;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using BitBracket.Controllers;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Security.Claims;
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace BitBracket_NUnit_Tests
{
    public class UserAnnouncementsApiTests
    {
        private Mock<UserManager<BitUser>> _userManagerMock;
        private Mock<IUserAnnouncementRepository> _announcementRepoMock;
        private UserAnnouncementsApiController _controller;

        [SetUp]
        public void Setup()
        {
            _userManagerMock = new Mock<UserManager<BitUser>>(
                Mock.Of<IUserStore<BitUser>>(), null, null, null, null, null, null, null, null);

            _announcementRepoMock = new Mock<IUserAnnouncementRepository>();

            _controller = new UserAnnouncementsApiController(_announcementRepoMock.Object, _userManagerMock.Object);

            // Mock User Identity
            var user = new ClaimsPrincipal(new GenericPrincipal(new GenericIdentity("TestUser"), null));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        [Test]
        public async Task CreateAnnouncement_ShouldReturnOk_WhenAnnouncementIsValid()
        {
            // Arrange
            var announcement = new Announcement { Title = "Test", Description = "Test Description", BitUserId = 1 };
            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(new BitUser { Id = 1, Username = "TestUser" });

            // Act
            var result = await _controller.Create(announcement);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            _announcementRepoMock.Verify(x => x.AddAsync(It.IsAny<Announcement>()), Times.Once);
        }

        [Test]
        public async Task GetDrafts_ShouldReturnAnnouncements_WhenUserHasDrafts()
        {
            // Arrange
            var expectedAnnouncements = new List<Announcement>
            {
                new Announcement { Title = "Draft 1", IsDraft = true },
                new Announcement { Title = "Draft 2", IsDraft = true }
            };

            _userManagerMock.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                            .ReturnsAsync(new BitUser { Id = 1, Username = "TestUser" });
            _announcementRepoMock.Setup(x => x.GetByUserIdAndStatus(1, true))
                                 .ReturnsAsync(expectedAnnouncements);

            // Act
            var result = await _controller.GetDrafts();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var announcements = okResult.Value.Should().BeAssignableTo<IEnumerable<Announcement>>().Subject;
            announcements.Should().HaveCount(expectedAnnouncements.Count);
        }
    }
}
