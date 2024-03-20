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

        var tournaments = await _tournamentRepository.GetAllByOwnerId(owner.Id);

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
                TournamentId = model.TournamentId
            };

            // Your code to save the bracket...
            await _bracketRepository.Add(bracket);

            return Ok(bracket);
        }

        return BadRequest(ModelState);
    }



}