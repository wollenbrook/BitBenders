//DAL/Concrete/TournamentRepository.cs

using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using BitBracket.DAL.Concrete;
using HW6.DAL.Concrete;

namespace BitBracket.DAL.Concrete
{
    public class TournamentRepository : Repository<Tournament>, ITournamentRepository
    {
        private readonly DbSet<Tournament> _tournaments;
        private readonly BitBracketDbContext _context;

        public TournamentRepository(BitBracketDbContext context): base (context)
        {
            _context = context;
            _tournaments = context.Tournaments;
        }

        public async Task Add(Tournament tournament)
        {
            _tournaments.Add(tournament);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tournament = await _tournaments.FindAsync(id);
            if (tournament == null)
            {
                throw new WebException("Tournament not found");
            }
            _tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
        }

        public async Task<Tournament> Get(int id)
        {
            return await _tournaments.FindAsync(id);
        }

        public async Task<IEnumerable<Tournament>> GetAll()
        {
            return await _tournaments.ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetAllByOwnerId(int ownerId)
        {
            return await _context.Tournaments.Where(t => t.Owner == ownerId).ToListAsync();
        }

        public async Task Update(Tournament tournament)
        {
            _tournaments.Update(tournament);
            await _context.SaveChangesAsync();
        }
    //

        public async Task<bool> SendParticipateRequest(int userId, int tournamentId)
        {
            var exists = await _context.ParticipateRequests.AnyAsync(r => r.SenderId == userId && r.TournamentId == tournamentId);
            if (!exists)
            {
                _context.ParticipateRequests.Add(new ParticipateRequest
                {
                    SenderId = userId,
                    TournamentId = tournamentId,
                    Status = "Pending"
                });
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> AcceptParticipateRequest(int requestId)
        {
            var request = await _context.ParticipateRequests.FindAsync(requestId);
            if (request != null && request.Status == "Pending")
            {
                request.Status = "Approved";
                _context.Participates.Add(new Participate
                {
                    UserId = request.SenderId,
                    TournamentId = request.TournamentId
                });
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeclineParticipateRequest(int requestId)
        {
            var request = await _context.ParticipateRequests.FindAsync(requestId);
            if (request != null && request.Status == "Pending")
            {
                request.Status = "Rejected";
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveParticipate(int userId, int tournamentId)
        {
            var participate = await _context.Participates.FirstOrDefaultAsync(p => p.UserId == userId && p.TournamentId == tournamentId);
            if (participate != null)
            {
                _context.Participates.Remove(participate);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfParticipates(int userId, int tournamentId)
        {
            return await _context.Participates.AnyAsync(p => p.UserId == userId && p.TournamentId == tournamentId);
        }

        public async Task<IEnumerable<ParticipateRequest>> GetParticipateRequests(int tournamentId)
        {
            return await _context.ParticipateRequests
                .Where(p => p.TournamentId == tournamentId)
                .Include(p => p.Sender)
                .ToListAsync();
        }

        public async Task<IEnumerable<Participate>> GetParticipates(int tournamentId)
        {
            return await _context.Participates
                .Where(p => p.TournamentId == tournamentId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsByUserId(int userId)
        {
            return await _context.Participates
                .Where(p => p.UserId == userId)
                .Select(p => p.Tournament)
                .Include(t => t.OwnerNavigation)
                .ToListAsync();
        }

        public async Task<bool> WithdrawFromTournament(int userId, int tournamentId)
        {
            var participation = await _context.Participates
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TournamentId == tournamentId);
            if (participation != null)
            {
                _context.Participates.Remove(participation);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}