using BitBracket.Controllers;
using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BitBracket_NUnit_Tests
{
    public class BitUserManagementTests
    {
        private BitUserApiController _controller;
        private Mock<IBitUserRepository> _mockBitUserRepo;
        private Mock<UserManager<IdentityUser>> _mockUserManager;
        private Mock<BitBracketDbContext> _mockContext;
        [SetUp]
        public void Setup()
        {
            _mockBitUserRepo = new Mock<IBitUserRepository>();
            var store = new Mock<IUserStore<IdentityUser>>();
            BitUser testBitUser = new BitUser { AspnetIdentityId = "randomid", Tag = "test", Bio = "test" };
            _mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            _mockContext = new Mock<BitBracketDbContext>();
            _mockUserManager.Setup(repo => repo.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns("randomid");
            _mockBitUserRepo.Setup(repo => repo.GetBitUserByEntityId(It.IsAny<string>())).Returns(testBitUser);
            var bitUsers = new List<BitUser>
            {
                new BitUser { AspnetIdentityId = "randomid", Tag = "test" }
            }.AsQueryable();

            var mockDbSet = new Mock<DbSet<BitUser>>();
            mockDbSet.As<IQueryable<BitUser>>().Setup(m => m.Provider).Returns(bitUsers.Provider);
            mockDbSet.As<IQueryable<BitUser>>().Setup(m => m.Expression).Returns(bitUsers.Expression);
            mockDbSet.As<IQueryable<BitUser>>().Setup(m => m.ElementType).Returns(bitUsers.ElementType);
            mockDbSet.As<IQueryable<BitUser>>().Setup(m => m.GetEnumerator()).Returns(() => bitUsers.GetEnumerator());

            _controller = new BitUserApiController(_mockContext.Object, _mockUserManager.Object, _mockBitUserRepo.Object);

        }
        [Test]
        public async Task TagIsChangedProperly()
        {
            string id = "randomid";
            string tag = "newTag";
            var result = await _controller.UpdateBitUserTag(tag);
            BitUser bitUser = _mockBitUserRepo.Object.GetBitUserByEntityId(id);
            Assert.That(bitUser.Tag, Is.EqualTo("newTag"));
        }
        [Test]
        public async Task BioIsChangedProperly()
        {
            string id = "randomid";
            string bio = "newBio";
            var result = await _controller.UpdateBitUserBio(bio);
            BitUser bitUser = _mockBitUserRepo.Object.GetBitUserByEntityId(id);
            Assert.That(bitUser.Bio, Is.EqualTo("newBio"));
        }
        [Test]
        public async Task BadRequestResultIfEmptyFileIsUploaded()
        {
            string id = "randomid";
            IFormFile test = null;

            var result = await _controller.UploadProfilePicture(test);
            BitUser bitUser = _mockBitUserRepo.Object.GetBitUserByEntityId(id);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }
        [Test]
        public async Task ProfilePictureIsUploadedProperly()
        {
            string id = "randomid";
            byte[] fileContent = Encoding.UTF8.GetBytes("This is the content of the file");

            // Create a MemoryStream from the file content
            MemoryStream stream = new MemoryStream(fileContent);

            // Create the FormFile object with the MemoryStream and other parameters
            IFormFile test = new FormFile(stream, 0, stream.Length, "test", "test");
            var result = await _controller.UploadProfilePicture(test);
            Assert.IsInstanceOf<OkResult>(result);
        }
        [Test]
        public async Task InvalidProfilePictureLengthReturnsBadResult()
        {
            string id = "randomid";
            byte[] fileContent = Encoding.UTF8.GetBytes("This is the content of the file");

            MemoryStream stream = new MemoryStream(fileContent);

            IFormFile test = new FormFile(stream, 0, 0, "test", "test");
            var result = await _controller.UploadProfilePicture(test);
            Assert.IsInstanceOf<BadRequestResult>(result);
        }

    }
}
