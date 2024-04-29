using BitBracket.Models;
using BitBracket.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BitBracket.Controllers;

public class BracketController : Controller
{
    [HttpGet]
    public IActionResult CreateBracket()
    {
        var model = new BasicBracketViewModel();
        return View(model);
    }

    public IActionResult TournamentDashboard()
    {
        return View();
    }

    public IActionResult TournamentPage(int id)
    {
        return View((object)id);
    }

    public IActionResult BracketPage(int id)
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