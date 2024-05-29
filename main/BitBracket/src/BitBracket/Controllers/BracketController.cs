using BitBracket.Models;
using BitBracket.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using BitBracket.DAL.Abstract;


namespace BitBracket.Controllers;

public class BracketController : Controller
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly IBitUserRepository _bitUserRepository;
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IBracketRepository _bracketRepository;

    public BracketController(UserManager<IdentityUser> userManager, IBitUserRepository bitUserRepository, ITournamentRepository tournamentRepository, IBracketRepository bracketRepository)
    {
        _userManager = userManager;
        _bitUserRepository = bitUserRepository;
        _tournamentRepository = tournamentRepository;
        _bracketRepository = bracketRepository;
    }
    [HttpGet]
    public IActionResult CreateBracket()
    {
        var model = new BasicBracketViewModel();
        return View(model);
    }

    [Authorize]
    public IActionResult TournamentDashboard()
    {
        if (!User.Identity.IsAuthenticated)
        {
                TempData["Alert"] = "You need to be logged in to access this page.";
                return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [Authorize]
    public async Task<IActionResult> TournamentPage(int id)
    {
        string userId = _userManager.GetUserId(User);
        var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);
        Tournament tournament = await _tournamentRepository.Get(id);

        if (tournament.Owner != bitUser.Id)
        {
            TempData["Alert"] = "You've been moved to the front page as you are no longer on the account that created this tournament, to revist this page you must log into the account that created this tournament.";
            return RedirectToAction("Index", "Home");
        }

        return View((object)id);
    }

    [Authorize]
    public async Task<IActionResult> BracketPage(int id)
    {
        var userId = _userManager.GetUserId(User);
        var bitUser = _bitUserRepository.GetBitUserByEntityId(userId);
        Bracket bracket = await _bracketRepository.Get(id);
        Tournament tournament = await _tournamentRepository.Get(bracket.TournamentID.Value); // Convert nullable int to non-nullable int

        if (tournament.Owner != bitUser.Id)
        {
            TempData["Alert"] = "You've been moved to the front page as you are no longer on the account that created this tournament bracket, to revist this page you must log into the account that created this tournament bracket";
            return RedirectToAction("Index", "Home");
        }
        TournamentViewModel tournamentViewModel = new TournamentViewModel
        {
            TournamentId = tournament.Id,
        };
        return View( tournamentViewModel);
    }

    public IActionResult BracketView(int id)
    {
        return View((object)id);
    }

    [HttpPost]
    public IActionResult CreateBracket(BasicBracketViewModel model)
    {
        if (ModelState.IsValid)
        {
            List<string> playerNames = model.Names.Split(',').Select(name => name.Trim()).ToList();

            if (playerNames.Count < 2)
            {
                ModelState.AddModelError("Names", "You must enter at least 2 names.");
                return View(model);
            }

            List<Player> players = new List<Player>();

            if (model.RandomSeeding)
            {
                // Shuffle player names for random seeding
                Random random = new Random();
                playerNames = playerNames.OrderBy(x => random.Next()).ToList();
            }

            for (int i = 0; i < playerNames.Count; i++)
            {
                players.Add(new Player
                {
                    PlayerID = i + 1,
                    PlayerTag = playerNames[i],
                    PlayerSeed = i + 1 // Set seed based on order of player names
                });
            }
            // change this
            TempData["Players"] = System.Text.Json.JsonSerializer.Serialize(players);
        }

        return View(model);
    }

    public IActionResult GuestBracketView(Guid id)
    {
        return View((object)id); 
    }
}