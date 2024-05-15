//TournamentApiController.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BitBracket.Models;
using BitBracket.ViewModels;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;
using BitBracket.DAL.Concrete;
using Newtonsoft.Json.Linq;

namespace BitBracket.Controllers;

//  /api/TournamentAPI
[Route("api/[controller]")]
[ApiController]
public class TournamentAPIController : ControllerBase
{
    private readonly ITournamentRepository _tournamentRepository;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBracketRepository _bracketRepository;
    private readonly IBitUserRepository _bitUserRepository;

    public TournamentAPIController(ITournamentRepository tournamentRepository, UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository, IBracketRepository bracketRepository)
    {
        _tournamentRepository = tournamentRepository;
        _userManager = userManager;
        _bitUserRepository = bitUserRepository;
        _bracketRepository = bracketRepository;
    }

    // /api/TournamentAPI/All/
    [HttpGet]
    [Route("All")]

    public async Task<IActionResult> GetAllTournaments()
    {

        var tournaments = await _tournamentRepository.GetAll();

        if (tournaments == null)
        {
            return NotFound();
        }

        return Ok(tournaments);
    }

    //  /api/TournamentAPI/Search/{Name}
    [HttpGet]
    [Route("Search/{Name}")]
    public async Task<IActionResult> GetTournamentsByName(string Name)
    {
        var tournaments = await _tournamentRepository.GetByName(Name);

        if (tournaments == null)
        {
            return NotFound();
        }

        return Ok(tournaments);
    }
    //  /api/TournamentAPI
    [HttpGet]
    public async Task<IActionResult> GetTournamentsByOwner()
    {
        var userId = _userManager.GetUserId(User);
        var owner = _bitUserRepository.GetBitUserByEntityId(userId);
        
        if (owner == null)
        {
            return NotFound("User not found");
        }

        var tournaments = await _tournamentRepository.GetAllByOwnerId(owner.Id );

        if (tournaments == null)
        {
            return NotFound("No tournaments found for this user");
        }

        return Ok(tournaments);
    }

    //  /api/TournamentAPI/bracket/{id}
    [HttpGet]
    [Route("Bracket/{id}")]
    public async Task<IActionResult> GetBracketsByTournamentId(int id)
    {

        if (id == null)
        {
            return NotFound();
        }
        
        var brackets = await _bracketRepository.GetAllByTournamentId(id);

        if (brackets == null)
        {
            return NotFound();
        }

        return Ok(brackets);
    }

    //  /api/TournamentAPI/bracket/{id}
    [HttpGet]
    [Route("bracket/display/{id}")]
    public async Task<IActionResult> GetBracket(int id)
    {
        var bracket = await _bracketRepository.Get(id);

        if (bracket == null)
        {
            return NotFound();
        }

        return Ok(bracket);
    }

    //  /api/TournamentAPI/{id}
    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetTournament(int id)
    {
        var tournament = await _tournamentRepository.Get(id);

        if (tournament == null)
        {
            return NotFound();
        }

        return Ok(tournament);
    }

    //  /api/TournamentAPI/User/{id}
    [HttpGet]
    [Route("User/{id}")]
    public async Task<IActionResult> GetOwnerTag(int id)
    {
        var user = _bitUserRepository.GetBitUserByRegularId(id);

        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] TournamentViewModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = _userManager.GetUserId(User);
            var owner = _bitUserRepository.GetBitUserByEntityId(userId);

            var tournament = new Tournament
            {
                Name = model.Name,
                Location = model.Location,
                Status = "In-Progress",
                Created = DateTime.UtcNow,
                Owner = owner.Id
            };

            await _tournamentRepository.Add(tournament);

            return Ok(tournament);
        }

        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("Create/Bracket")]
    public async Task<IActionResult> CreateBracket([FromBody] BracketViewModel model)
    {
        if (ModelState.IsValid)
        {
            
            // Create a new Bracket object from the model
            var bracket = new Bracket
            {
                Name = model.BracketName,
                Status = "In-Progress",
                BracketData = model.BracketData,
                TournamentID = model.TournamentId,
                UserBracket = model.IsUserBracket
            };

            // Your code to save the bracket...
            await _bracketRepository.Add(bracket);

            return Ok(bracket);
        }

        return BadRequest(ModelState);
    }


    [HttpPut]
    [Route("Bracket/Update")]
    public async Task<IActionResult> UpdateBracket([FromBody] BracketUpdateViewModel model)
    {
        if (ModelState.IsValid)
        {
            var bracket = await _bracketRepository.Get(model.BracketId);

            if (bracket == null)
            {
                return NotFound();
            }
            bracket.BracketData = model.BracketData;

            await _bracketRepository.Update(bracket);

            return Ok(bracket);
        }

        return BadRequest(ModelState);
    }


    [HttpPut]
    [Route("Broadcast")]
    public async Task<IActionResult> UpdateBroadcastLink([FromBody] BroadcastLinkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tournament = await _tournamentRepository.Get(model.TournamentId);
        if (tournament == null)
        {
            return NotFound();
        }

        tournament.BroadcastType = model.BroadcastType;
        tournament.BroadcastLink = model.NameOrID;

        await _tournamentRepository.Update(tournament);

        return NoContent();
    }


//
    // [HttpGet("All")]
    // public async Task<ActionResult<IEnumerable<Tournament>>> AllTournaments()
    // {
    //     return Ok(await _tournamentRepository.AllTournaments());
    // }

    [HttpPost("SendRequest/{userId}/{tournamentId}")]
    public async Task<IActionResult> SendParticipateRequest(int userId, int tournamentId)
    {
        bool result = await _tournamentRepository.SendParticipateRequest(userId, tournamentId);
        if (result)
            return Ok(new { message = "Request sent successfully!" });
        else
            return BadRequest(new { message = "Request already exists or failed to send." });
    }

    [HttpPut("AcceptRequest/{requestId}")]
    public async Task<IActionResult> AcceptParticipateRequest(int requestId)
    {
        bool result = await _tournamentRepository.AcceptParticipateRequest(requestId);
        if (result)
            return Ok(new { message = "Request accepted successfully!" });
        else
            return NotFound();
    }

    [HttpPut("DeclineRequest/{requestId}")]
    public async Task<IActionResult> DeclineParticipateRequest(int requestId)
    {
        bool result = await _tournamentRepository.DeclineParticipateRequest(requestId);
        if (result)
            return Ok(new { message = "Request declined successfully!" });
        else
            return NotFound();
    }


    [HttpGet("GetParticipates/{tournamentId}")]
    public async Task<IActionResult> GetParticipates(int tournamentId)
    {
        var participants = await _tournamentRepository.GetParticipates(tournamentId);
        var participantDtos = participants.Select(p => new {
            UserId = p.UserId,
            Username = p.User.Username
        }).ToList();
        return Ok(participantDtos);
    }

    [HttpGet("GetParticipateRequests/{tournamentId}")]
    public async Task<IActionResult> GetParticipateRequests(int tournamentId)
    {
        var requests = await _tournamentRepository.GetParticipateRequests(tournamentId);
        var requestDtos = requests.Select(r => new {
            RequestId = r.Id,
            SenderId = r.SenderId,
            SenderUsername = r.Sender.Username,
            Status = r.Status
        }).ToList();
        return Ok(requestDtos);
    }

    [HttpPut("RemoveParticipate/{userId}/{tournamentId}")]
    public async Task<IActionResult> RemoveParticipate(int userId, int tournamentId)
    {
        bool result = await _tournamentRepository.RemoveParticipate(userId, tournamentId);
        if (result)
            return Ok(new { message = "Participant removed successfully!" });
        else
            return NotFound(new { message = "Participant not found or could not be removed." });
    }


    [HttpGet("CheckIfParticipates/{userId}/{tournamentId}")]
    public async Task<IActionResult> CheckIfParticipates(int userId, int tournamentId)
    {
        bool isParticipating = await _tournamentRepository.CheckIfParticipates(userId, tournamentId);
        return Ok(new { isParticipating });
    }

//
    // [HttpGet("UserTournaments")]
    // public async Task<IActionResult> GetUserTournaments()
    // {
    //     var userId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value);  // Ensure you get the correct user ID from claims
    //     var tournaments = await _tournamentRepository.GetTournamentsByUserId(userId);
    //     return Ok(tournaments.Select(t => new 
    //     {
    //         Id = t.Id,
    //         Name = t.Name,
    //         Location = t.Location,
    //         Status = t.Status
    //     }));
    // }

    // [HttpPost("Withdraw/{tournamentId}")]
    // public async Task<IActionResult> WithdrawFromTournament(int tournamentId)
    // {
    //     var userId = int.Parse(User.Claims.First(c => c.Type == "UserID").Value); // Retrieve user ID from claims
    //     bool result = await _tournamentRepository.WithdrawFromTournament(userId, tournamentId);
    //     if (result)
    //     {
    //         return Ok();
    //     }
    //     return BadRequest("Could not withdraw from the tournament.");
    // }
}