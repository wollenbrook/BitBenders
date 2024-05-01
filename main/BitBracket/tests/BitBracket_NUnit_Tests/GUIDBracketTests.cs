using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System;
using BitBracket.Controllers;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using BitBracket.Models;
using BitBracket.ViewModels;

namespace BitBracket_NUnit_Tests
{
    
    public class GuidBracketRepositoryTests
    {
        private Mock<IGuidBracketRepository> _repository;
        private Mock<BitBracketDbContext> _context;
        private GuidBracket _guidBracket;
        private GuidBracketViewModel _viewModel;
        private GuidAPIController _controller;

        [SetUp]
        public void Setup()
        {
            _context = new Mock<BitBracketDbContext>();
            _repository = new Mock<IGuidBracketRepository>();
            _guidBracket = new GuidBracket();
            _viewModel = new GuidBracketViewModel();
        }

        [Test]
        public void GuidBracket_ShouldInitializeCorrectly()
        {
            Assert.IsNotNull(_guidBracket);
        }

        // Add more tests for other properties and methods in GuidBracket

        [Test]
        public void GuidBracketViewModel_ShouldInitializeCorrectly()
        {
            Assert.IsNotNull(_viewModel);
        }

        // Add more tests for other properties and methods in GuidBracketViewModel
/*
        [Test]
        public async Task AddGuidBracket_ShouldAddGuidBracket()
        {
            var guidBracket = new GuidBracket();
            await _repository.AddGuidBracket(guidBracket);
            _context.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

*/
        // Add more tests for other methods in GuidBracketRepository

        [Test]
        public async Task GetGuidBracket_ShouldReturnGuidBracket()
        {
            var guid = Guid.NewGuid();
            var guidBracket = new GuidBracket();
            guidBracket.Guid = guid;
            _repository.Setup(x => x.GetGuidBracket(guid)).ReturnsAsync(guidBracket);

            var result = await _controller.GetGuidBracket(guid);

            Assert.AreEqual(guidBracket, result);
        }

        // Add more tests for other actions in GUIDApiController
    }
}