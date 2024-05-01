//Controllers/TournamentPlayersApiController.cs

using Microsoft.AspNetCore.Mvc;
using BitBracket.DAL.Abstract;
using System.Threading.Tasks;
using BitBracket.Models;

namespace BitBracket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentPlayersApiController : ControllerBase
    {
        private readonly ITournamentPlayerRepository _repository;

        public TournamentPlayersApiController(ITournamentPlayerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("tournaments")]
        public async Task<IActionResult> GetAllTournaments()
        {
            var tournaments = await _repository.GetAllTournamentsWithDetailsAsync();
            return Ok(tournaments);
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinTournament([FromBody] JoinedPlayer player)
        {
            await _repository.AddPlayerToTournamentAsync(player);
            return Ok();
        }

        [HttpGet("tournament/{id}/players")]
        public async Task<IActionResult> GetPlayers(int id)
        {
            var players = await _repository.GetPlayersByTournamentIdAsync(id);
            return Ok(players);
        }

        [HttpDelete("tournament/{tournamentId}/player/{playerId}")]
        public async Task<IActionResult> RemovePlayer(int tournamentId, int playerId)
        {
            await _repository.RemovePlayerFromTournamentAsync(playerId, tournamentId);
            return Ok();
        }
    }
}
