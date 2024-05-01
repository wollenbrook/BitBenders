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
    }
}