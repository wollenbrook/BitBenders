// using NUnit.Framework;
// using Moq;
// using Microsoft.AspNetCore.Mvc;
// using BitBracket.Controllers;
// using BitBracket.DAL.Abstract;
// using Microsoft.AspNetCore.Identity;
// using BitBracket.Models;
// using System.Threading.Tasks;
// using System.Collections.Generic;
// using System.Security.Claims;
// using System.Security.Principal;
// using Microsoft.AspNetCore.Http;

// [TestFixture]
// public class ParticipateTests
// {
//     private TournamentAPIController _controller;
//     private Mock<ITournamentRepository> _mockRepo;
//     private Mock<UserManager<IdentityUser>> _mockUserManager;

//     [SetUp]
//     public void Setup()
//     {
//         // Mock the repository and UserManager
//         _mockRepo = new Mock<ITournamentRepository>();
//         var userStoreMock = new Mock<IUserStore<IdentityUser>>();
//         _mockUserManager = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

//         // Set up a ClaimsPrincipal with a UserID claim for testing
//         var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//         {
//             new Claim("UserID", "1")
//         }));

//         // Instantiate the controller and set the HttpContext to mimic authentication
//         _controller = new TournamentAPIController(_mockRepo.Object, _mockUserManager.Object, null, null)
//         {
//             ControllerContext = new ControllerContext()
//             {
//                 HttpContext = new DefaultHttpContext() { User = user }
//             }
//         };
//     }

//     [Test]
//     public async Task WithdrawFromTournament_WhenCalled_ReturnsOkResult()
//     {
//         // Arrange: Set up conditions and responses for the test case
//         var tournamentId = 1;
//         _mockRepo.Setup(repo => repo.WithdrawFromTournament(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Perform the operation being tested
//         var result = await _controller.WithdrawFromTournament(tournamentId);

//         // Assert: Verify the outcome of the test
//         Assert.That(result, Is.TypeOf<OkResult>());
//     }

//     [Test]
//     public async Task WithdrawFromTournament_WhenCalled_Fails_ReturnsBadRequest()
//     {
//         // Arrange: Prepare for a scenario where the withdrawal fails
//         var tournamentId = 1;
//         _mockRepo.Setup(repo => repo.WithdrawFromTournament(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(false);

//         // Act: Attempt to withdraw from a tournament
//         var result = await _controller.WithdrawFromTournament(tournamentId);

//         // Assert: Check that the method returns a BadRequest result
//         Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
//     }

//     [Test]
//     public async Task GetParticipates_WhenCalled_ReturnsListOfParticipants()
//     {
//         // Arrange: Expectation for the repository to return a list of participates
//         var tournamentId = 1;
//         _mockRepo.Setup(repo => repo.GetParticipates(tournamentId)).ReturnsAsync(new List<Participate>
//         {
//             new Participate { UserId = 1, TournamentId = 1 },
//             new Participate { UserId = 2, TournamentId = 1 }
//         });

//         // Act: Request the list of participants for a tournament
//         var result = await _controller.GetParticipates(tournamentId);

//         // Assert: Verify that the result is correct and contains the expected number of items
//         var okResult = result as OkObjectResult;
//         Assert.IsNotNull(okResult);
//         var items = okResult.Value as List<dynamic>;
//         Assert.AreEqual(2, items.Count);
//     }

//     [Test]
//     public async Task SendParticipateRequest_ValidRequest_ReturnsOk()
//     {
//         // Arrange: Expectation for a successful participate request send
//         _mockRepo.Setup(repo => repo.SendParticipateRequest(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Attempt to send a participate request
//         var result = await _controller.SendParticipateRequest(1, 1);

//         // Assert: Verify successful response
//         Assert.That(result, Is.TypeOf<OkObjectResult>());
//     }

//     [Test]
//     public async Task AcceptParticipateRequest_ValidRequest_ReturnsOk()
//     {
//         // Arrange: Set up the repository to confirm request acceptance
//         _mockRepo.Setup(repo => repo.AcceptParticipateRequest(It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Attempt to accept a participate request
//         var result = await _controller.AcceptParticipateRequest(1);

//         // Assert: Verify that the response is successful
//         Assert.That(result, Is.TypeOf<OkObjectResult>());
//     }

//     [Test]
//     public async Task DeclineParticipateRequest_ValidRequest_ReturnsOk()
//     {
//         // Arrange: Set up the repository to confirm request decline
//         _mockRepo.Setup(repo => repo.DeclineParticipateRequest(It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Attempt to decline a participate request
//         var result = await _controller.DeclineParticipateRequest(1);

//         // Assert: Check for a successful decline response
//         Assert.That(result, Is.TypeOf<OkObjectResult>());
//     }

//     [Test]
//     public async Task RemoveParticipate_ValidRequest_ReturnsOk()
//     {
//         // Arrange: Set up conditions for a successful removal
//         _mockRepo.Setup(repo => repo.RemoveParticipate(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Attempt to remove a participant
//         var result = await _controller.RemoveParticipate(1, 1);

//         // Assert: Verify the removal was successful
//         Assert.That(result, Is.TypeOf<OkObjectResult>());
//     }

//     [Test]
//     public async Task CheckIfParticipates_Participates_ReturnsTrue()
//     {
//         // Arrange: Set up the repository to return true for participation check
//         _mockRepo.Setup(repo => repo.CheckIfParticipates(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(true);

//         // Act: Check participation status
//         var result = await _controller.CheckIfParticipates(1, 1);

//         // Assert: Ensure the result indicates participation
//         var okResult = result as OkObjectResult;
//         Assert.IsNotNull(okResult);
//         Assert.IsTrue((bool)okResult.Value.GetType().GetProperty("isParticipating").GetValue(okResult.Value, null));
//     }

//     [Test]
//     public async Task GetParticipateRequests_ValidRequest_ReturnsRequests()
//     {
//         // Arrange: Set up expectations for retrieving participate requests
//         _mockRepo.Setup(repo => repo.GetParticipateRequests(It.IsAny<int>())).ReturnsAsync(new List<ParticipateRequest>
//         {
//             new ParticipateRequest { Id = 1, SenderId = 1, TournamentId = 1, Status = "Pending" }
//         });

//         // Act: Retrieve participate requests
//         var result = await _controller.GetParticipateRequests(1);

//         // Assert: Verify correct retrieval and count
//         var okResult = result as OkObjectResult;
//         Assert.IsNotNull(okResult);
//         var items = okResult.Value as List<dynamic>;
//         Assert.AreEqual(1, items.Count);
//     }

//     [Test]
//     public async Task GetUserTournaments_ValidUser_ReturnsTournaments()
//     {
//         // Arrange: Simulate fetching tournaments for a user
//         _mockRepo.Setup(repo => repo.GetTournamentsByUserId(It.IsAny<int>())).ReturnsAsync(new List<Tournament>
//         {
//             new Tournament { Id = 1, Name = "Tournament 1", Location = "Location 1", Status = "Active" }
//         });

//         // Act: Retrieve tournaments for the user
//         var result = await _controller.GetUserTournaments();

//         // Assert: Verify that the correct tournaments are returned
//         var okResult = result as OkObjectResult;
//         Assert.IsNotNull(okResult);
//         var tournaments = okResult.Value as List<dynamic>;
//         Assert.AreEqual(1, tournaments.Count);
//     }
// }
