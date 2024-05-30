using NUnit.Framework; // Importing the NUnit framework for unit testing
using Moq; // Importing Moq for creating mock objects
using BitBracket.Controllers; // Importing the TournamentAPIController
using BitBracket.DAL.Abstract; // Importing the interfaces for repositories
using Microsoft.AspNetCore.Identity; // Importing Identity for user management
using Microsoft.AspNetCore.Mvc; // Importing MVC for IActionResult
using System.Threading.Tasks; // Importing Task for asynchronous programming
using BitBracket.Models; // Importing the models used in the controller
using System.Collections.Generic; // Importing generic collections
using Microsoft.Extensions.Logging; // Importing logging
using Microsoft.AspNetCore.Http; // Importing HTTP context
using Microsoft.Extensions.Options; // Importing options pattern
using System.Security.Claims; // Importing claims for user identity

namespace BitBracket_NUnit_Tests // Defining the namespace for the test class
{
    [TestFixture] // Marking this class as a test fixture for NUnit
    public class TournamentAPIControllerTests
    {
        private Mock<ITournamentRepository> _tournamentRepositoryMock; // Mock for the ITournamentRepository
        private Mock<UserManager<IdentityUser>> _userManagerMock; // Mock for the UserManager
        private Mock<IBitUserRepository> _bitUserRepositoryMock; // Mock for the IBitUserRepository
        private Mock<IBracketRepository> _bracketRepositoryMock; // Mock for the IBracketRepository
        private TournamentAPIController _controller; // Instance of the controller being tested

        [SetUp] // This method runs before each test
        public void SetUp()
        {
            // Initializing the mocks
            _tournamentRepositoryMock = new Mock<ITournamentRepository>();
            _userManagerMock = MockUserManager<IdentityUser>();
            _bitUserRepositoryMock = new Mock<IBitUserRepository>();
            _bracketRepositoryMock = new Mock<IBracketRepository>();

            // Initializing the controller with the mocked dependencies
            _controller = new TournamentAPIController(_tournamentRepositoryMock.Object, _userManagerMock.Object, _bitUserRepositoryMock.Object, _bracketRepositoryMock.Object);
        }

        // Helper method to create a mock UserManager
        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            // Creating a mock for the user store
            var store = new Mock<IUserStore<TUser>>();
            // Creating the UserManager mock with the necessary constructor arguments
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            return mgr; // Returning the mock
        }

        [Test] // Marking this method as a test case
        public async Task AcceptParticipationRequest_ValidRequest_ReturnsOk()
        {
            // Arrange: setting up the mock repository to return true for a valid request
            var requestId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.AcceptParticipationRequestAsync(requestId)).ReturnsAsync(true);

            // Act: calling the AcceptParticipationRequest method on the controller
            var result = await _controller.AcceptParticipationRequest(requestId);

            // Assert: verifying that the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task AcceptParticipationRequest_InvalidRequest_ReturnsNotFound()
        {
            // Arrange: setting up the mock repository to return false for an invalid request
            var requestId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.AcceptParticipationRequestAsync(requestId)).ReturnsAsync(false);

            // Act: calling the AcceptParticipationRequest method on the controller
            var result = await _controller.AcceptParticipationRequest(requestId);

            // Assert: verifying that the result is a NotFoundObjectResult
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task DenyOrRemoveParticipationRequest_ValidRequest_ReturnsOk()
        {
            // Arrange: setting up the mock repository to return true for a valid request
            var requestId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.DenyOrRemoveParticipationRequestAsync(requestId)).ReturnsAsync(true);

            // Act: calling the DenyOrRemoveParticipationRequest method on the controller
            var result = await _controller.DenyOrRemoveParticipationRequest(requestId);

            // Assert: verifying that the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task DenyOrRemoveParticipationRequest_InvalidRequest_ReturnsNotFound()
        {
            // Arrange: setting up the mock repository to return false for an invalid request
            var requestId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.DenyOrRemoveParticipationRequestAsync(requestId)).ReturnsAsync(false);

            // Act: calling the DenyOrRemoveParticipationRequest method on the controller
            var result = await _controller.DenyOrRemoveParticipationRequest(requestId);

            // Assert: verifying that the result is a NotFoundObjectResult
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task RemoveParticipant_ValidRequest_ReturnsOk()
        {
            // Arrange: setting up the mock repository to return true for a valid request
            var userId = 1;
            var tournamentId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.RemoveParticipantAsync(userId, tournamentId)).ReturnsAsync(true);

            // Act: calling the RemoveParticipant method on the controller
            var result = await _controller.RemoveParticipant(userId, tournamentId);

            // Assert: verifying that the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task RemoveParticipant_InvalidRequest_ReturnsNotFound()
        {
            // Arrange: setting up the mock repository to return false for an invalid request
            var userId = 1;
            var tournamentId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.RemoveParticipantAsync(userId, tournamentId)).ReturnsAsync(false);

            // Act: calling the RemoveParticipant method on the controller
            var result = await _controller.RemoveParticipant(userId, tournamentId);

            // Assert: verifying that the result is a NotFoundObjectResult
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task WithdrawFromTournament_ValidRequest_ReturnsOk()
        {
            // Arrange: setting up the mock repository to return true for a valid request
            var userId = 1;
            var tournamentId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.WithdrawFromTournamentAsync(userId, tournamentId)).ReturnsAsync(true);

            // Act: calling the WithdrawFromTournament method on the controller
            var result = await _controller.WithdrawFromTournament(userId, tournamentId);

            // Assert: verifying that the result is an OkObjectResult
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test] // Marking this method as a test case
        public async Task WithdrawFromTournament_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange: setting up the mock repository to return false for an invalid request
            var userId = 1;
            var tournamentId = 1;
            _tournamentRepositoryMock.Setup(repo => repo.WithdrawFromTournamentAsync(userId, tournamentId)).ReturnsAsync(false);

            // Act: calling the WithdrawFromTournament method on the controller
            var result = await _controller.WithdrawFromTournament(userId, tournamentId);

            // Assert: verifying that the result is a BadRequestObjectResult
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
