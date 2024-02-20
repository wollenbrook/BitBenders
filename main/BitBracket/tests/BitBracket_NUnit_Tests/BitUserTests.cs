using BitBracket.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using BitBracket.DAL;
using BitBracket.DAL.Concrete;
using BitBracket.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BitBracket_NUnit_Tests;

public class Tests
{
    private AnnouncementsApiController _controller;
    private Mock<IAnnouncementRepository> _mockRepo;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IAnnouncementRepository>();
        _controller = new AnnouncementsApiController(_mockRepo.Object);
    }

    [Test]
    public void TestForCorrectReturnOfBitUserWithEntityId()
    {
        BitUser bituser = new BitUser();
        bituser.Username = "test";
        bituser.AspnetIdentityId = "randomid";
        var mockRepository = new Mock<IBitUserRepository>();
        string identityId = "randomid";
        BitUser expectedUser = new BitUser { Username = "test", AspnetIdentityId = identityId };

        mockRepository.Setup(repo => repo.GetBitUserByEntityId(identityId))
                      .Returns(expectedUser);

        IBitUserRepository repository = mockRepository.Object;

        BitUser actualUser = repository.GetBitUserByEntityId(identityId);

        Assert.AreEqual(expectedUser, actualUser);
    }
    [Test]
    public void TestForNoneReturnIfIdDoesNotExist()
    {

        var mockRepository = new Mock<IBitUserRepository>();
        string identityId = "randomid1";
        BitUser expectedUser1 = new BitUser { Username = "test", AspnetIdentityId = "randomid1" };
        BitUser expectedUser2 = new BitUser { Username = "test", AspnetIdentityId = "randomid2" };

        mockRepository.Setup(repo => repo.GetBitUserByEntityId(identityId))
                     .Returns(expectedUser1);

        IBitUserRepository repository = mockRepository.Object;

        BitUser actualUser = repository.GetBitUserByEntityId(identityId);
        Assert.AreNotEqual(expectedUser2, actualUser);
    }

            [Test]
        public async Task Create_ValidAnnouncement_ReturnsOk()
        {
            // Arrange
            var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };
            _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Announcement>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(newAnnouncement, "YourAdminKey");

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task Create_InvalidAdminKey_ReturnsUnauthorized()
        {
            // Arrange
            var newAnnouncement = new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" };

            // Act
            var result = await _controller.Create(newAnnouncement, "WrongKey");

            // Assert
            Assert.IsInstanceOf<UnauthorizedResult>(result);
        }

// Announcement Tests

        [Test]
        public async Task GetAll_ReturnsAllAnnouncements()
        {
            // Arrange
            var allAnnouncements = new List<Announcement> { new Announcement { Title = "Test", Description = "Test Description", Author = "Admin" } };
            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(allAnnouncements);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedAnnouncements = okResult.Value as IEnumerable<Announcement>;
            Assert.AreEqual(1, (returnedAnnouncements as List<Announcement>).Count);
        }
}




