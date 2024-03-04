using Microsoft.AspNetCore.Mvc;
using BitBracket.ViewModels;
using System.Text.Json;

namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BracketApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody] BasicBracketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List<string> playerNames = model.Names.Split(',').Select(name => name.Trim()).ToList();

            if (playerNames.Count < 2)
            {
                ModelState.AddModelError("Names", "You must enter at least 2 names.");
                return BadRequest(ModelState);
            }

            if (model.RandomSeeding)
            {
                // Shuffle player names for random seeding
                Random random = new Random();
                playerNames = playerNames.OrderBy(x => random.Next()).ToList();
            }

            // Convert the shuffled names back to a comma-separated string
            model.Names = string.Join(",", playerNames);

            // Convert the players list to JSON
            var json = JsonSerializer.Serialize(model);

            // Return the JSON
            return Ok(json);
        }
    }
}