using BitBracket.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using BitBracket.DAL;
using BitBracket.DAL.Concrete;
using BitBracket.DAL.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using BitBracket.Controllers;
using Microsoft.AspNetCore.Identity;

namespace BitBracket_NUnit_Tests
{



    public class BitUserSearchTests
    {
        private Mock<IBitUserRepository> _mockBitUserRepo;

        [SetUp]
        public void Setup()
        {
            _mockBitUserRepo = new Mock<IBitUserRepository>();

        }
        [Test]
        public void GetBitUserByRegularIdTest()
        {
            // Arrange
            int id = 1;
            BitUser expectedBitUser = new BitUser()
            {
                Username = "test",
                AspnetIdentityId = "randomid",
                Tag = "test",
                Bio = "test"
            };
            _mockBitUserRepo.Setup(repo => repo.GetBitUserByRegularId(id)).Returns(expectedBitUser);

            // Act
            BitUser actualBitUser = _mockBitUserRepo.Object.GetBitUserByRegularId(id);

            // Assert
            Assert.AreEqual(expectedBitUser, actualBitUser);
        }
        [Test]
        public void GetBitUserByNameTest()
        {
            // Arrange
            string username = "test";
            BitUser expectedBitUser = new BitUser()
            {
                Username = "test",
                AspnetIdentityId = "randomid",
                Tag = "test",
                Bio = "test"
            };
            _mockBitUserRepo.Setup(repo => repo.GetBitUserByName(username)).Returns(expectedBitUser);

            // Act
            BitUser actualBitUser = _mockBitUserRepo.Object.GetBitUserByName(username);

            // Assert
            Assert.AreEqual(expectedBitUser, actualBitUser);
        }
        [Test]
        public void GetBitUserByRegularId_NullTest()
        {
            // Arrange
            int id = 1;
            BitUser expectedBitUser = null;
            _mockBitUserRepo.Setup(repo => repo.GetBitUserByRegularId(id)).Returns(expectedBitUser);

            // Act
            BitUser actualBitUser = _mockBitUserRepo.Object.GetBitUserByRegularId(id);

            // Assert
            Assert.IsNull(actualBitUser);
        }
        [Test]
        public void GetBitUserByNameTestNull() {
            // Arrange
            string username = "test";
            BitUser expectedBitUser = null;
            _mockBitUserRepo.Setup(repo => repo.GetBitUserByName(username)).Returns(expectedBitUser);

            // Act
            BitUser actualBitUser = _mockBitUserRepo.Object.GetBitUserByName(username);

            // Assert
            Assert.IsNull(actualBitUser);
        }
    }
}