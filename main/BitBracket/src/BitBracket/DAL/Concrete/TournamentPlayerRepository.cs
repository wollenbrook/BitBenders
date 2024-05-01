using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBracket.DAL.Concrete
{
    public class TournamentPlayerRepository : ITournamentPlayerRepository
    {
        private readonly BitBracketDbContext _context;

        public TournamentPlayerRepository(BitBracketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tournament>> GetAllTournamentsWithDetailsAsync()
        {
            // Only include navigation properties
            return await _context.Tournaments
                .Include(t => t.OwnerNavigation)
                .Include(t => t.Players)
                .ToListAsync();
        }

        public async Task<Tournament> GetTournamentByIdAsync(int tournamentId)
        {
            return await _context.Tournaments
                .Include(t => t.OwnerNavigation)
                .Include(t => t.Players)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);
        }

        public async Task AddPlayerToTournamentAsync(JoinedPlayer player)
        {
            _context.JoinedPlayers.Add(player);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<JoinedPlayer>> GetPlayersByTournamentIdAsync(int tournamentId)
        {
            return await _context.JoinedPlayers
                .Include(j => j.Player)
                .Where(j => j.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task RemovePlayerFromTournamentAsync(int playerId, int tournamentId)
        {
            var player = await _context.JoinedPlayers
                .FirstOrDefaultAsync(p => p.PlayerId == playerId && p.TournamentId == tournamentId);
            if (player != null)
            {
                _context.JoinedPlayers.Remove(player);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SendJoinRequestAsync(SentJoinRequest request)
        {
            _context.SentJoinRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SentJoinRequest>> GetSentJoinRequestsByTournamentIdAsync(int tournamentId)
        {
            return await _context.SentJoinRequests
                .Include(r => r.PlayerSender)
                .Where(r => r.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<ReceivedPlayerRequest>> GetReceivedPlayerRequestsByTournamentIdAsync(int tournamentId)
        {
            return await _context.ReceivedPlayerRequests
                .Include(r => r.OwnerReceiver)
                .Where(r => r.TournamentId == tournamentId)
                .ToListAsync();
        }

        public async Task ApproveJoinRequestAsync(int requestId)
        {
            var request = await _context.ReceivedPlayerRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Approved";
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeclineJoinRequestAsync(int requestId)
        {
            var request = await _context.ReceivedPlayerRequests.FindAsync(requestId);
            if (request != null)
            {
                request.Status = "Declined";
                _context.Update(request);
                await _context.SaveChangesAsync();
            }
        }
    }
}
