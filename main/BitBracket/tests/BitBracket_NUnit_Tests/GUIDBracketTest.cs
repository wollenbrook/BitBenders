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

public class GUIDBracketTests
{
  private IGUIDBracketRepository _mockRepository;

    [SetUp]
    public void Setup()
    {
        _mockRepository = Mock.Of<IGUIDBracketRepository>();
        _generator = new BracketGUIDLinkGenerator();
    }

    [Test]
    public void TestGUIDGeneration_IsUnique()
    {
        var guid1 = _generator.GenerateGUID();
        var guid2 = _generator.GenerateGUID();

        Assert.AreNotEqual(guid1, guid2);
    }

    // Hard to properly test
    [Test]
    public void TestGUIDGeneration_IsNotGuessable()
    {
        var guessableGuids = new List<string>
    {
        "12345678-1234-1234-1234-123456789012",
        "98765432-4321-4321-4321-098765432109"
    };

    var generatedGuid = _generator.GenerateGUID();

    Assert.IsFalse(guessableGuids.Contains(generatedGuid));

    }

    [Test]
    public void TestBracketData_IsStoredCorrectly()
    {
        var guid = _generator.GenerateGUID();
        var bracketData = new BracketData();

        _mockRepository.StoreBracketData(guid, bracketData);

        var storedData = _mockRepository.GetBracketData(guid);

        Assert.AreEqual(bracketData, storedData);
    }

    [Test]
    public void TestBracketData_IsRetrievedCorrectly()
    {
        var guid = _generator.GenerateGUID();
        var bracketData = new BracketData();

        _mockRepository.StoreBracketData(guid, bracketData);

        var retrievedData = _mockRepository.GetBracketData(guid);

        Assert.AreEqual(bracketData, retrievedData);
    }

   
}
   