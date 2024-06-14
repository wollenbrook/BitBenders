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
        public async Task<IEnumerable<Tournament>> GetByName(string Name)
        {

            return await _tournaments.Where(t => t.Name.Contains(Name)).ToListAsync();
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
        
        public new async Task<IEnumerable<Tournament>> GetAll()
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

        public async Task<bool> SendParticipationRequestAsync(int userId, int tournamentId)
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

        public async Task<bool> AcceptParticipationRequestAsync(int requestId)
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

        public async Task<bool> DenyOrRemoveParticipationRequestAsync(int requestId)
        {
            var request = await _context.ParticipateRequests.FindAsync(requestId);
            if (request != null)
            {
                _context.ParticipateRequests.Remove(request);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveParticipantAsync(int userId, int tournamentId)
        {
            var participate = await _context.Participates
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TournamentId == tournamentId);
            if (participate != null)
            {
                // Remove the Participate entry
                _context.Participates.Remove(participate);

                // Also remove the corresponding ParticipateRequest entry
                var participateRequest = await _context.ParticipateRequests
                    .FirstOrDefaultAsync(pr => pr.SenderId == userId && pr.TournamentId == tournamentId && pr.Status == "Approved");
                if (participateRequest != null)
                {
                    _context.ParticipateRequests.Remove(participateRequest);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckIfParticipatesAsync(int userId, int tournamentId)
        {
            return await _context.ParticipateRequests.AnyAsync(p => p.SenderId == userId && p.TournamentId == tournamentId && (p.Status == "Approved" || p.Status == "Pending"));
        }

        public async Task<IEnumerable<Tournament>> GetTournamentsUserParticipatesInAsync(int userId)
        {
            return await _context.Participates
                .Where(p => p.UserId == userId)
                .Select(p => p.Tournament)
                .ToListAsync();
        }

        public async Task<bool> WithdrawFromTournamentAsync(int userId, int tournamentId)
        {
            var participation = await _context.Participates
                .FirstOrDefaultAsync(p => p.UserId == userId && p.TournamentId == tournamentId);
            if (participation != null)
            {
                // Remove the Participate entry
                _context.Participates.Remove(participation);

                // Also remove the corresponding ParticipateRequest entry
                var participateRequest = await _context.ParticipateRequests
                    .FirstOrDefaultAsync(pr => pr.SenderId == userId && pr.TournamentId == tournamentId && pr.Status == "Approved");
                if (participateRequest != null)
                {
                    _context.ParticipateRequests.Remove(participateRequest);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<IEnumerable<ParticipateRequest>> GetAllParticipationRequests(int tournamentId)
        {
            return await _context.ParticipateRequests
                                 .Where(pr => pr.TournamentId == tournamentId)
                                 .Include(pr => pr.Sender)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Participate>> GetApprovedParticipates(int tournamentId)
        {
            return await _context.Participates
                                 .Where(p => p.TournamentId == tournamentId)
                                 .Include(p => p.User)
                                 .ToListAsync();
        }

    }
}