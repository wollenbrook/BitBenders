using BitBracket.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Moq;
using BitBracket.DAL;
using BitBracket.DAL.Concrete;
using BitBracket.DAL.Abstract;

namespace BitBracket_NUnit_Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        
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
}




