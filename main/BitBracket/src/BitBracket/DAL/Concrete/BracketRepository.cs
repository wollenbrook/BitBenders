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

    public class BracketRepository : Repository<Bracket>, IBracketRepository
    {
        private readonly DbSet<Bracket> _brackets;
        private readonly BitBracketDbContext _context;

        public BracketRepository(BitBracketDbContext context) : base(context)
        {
            _context = context;
            _brackets = context.Brackets;
        }

        public async Task Add(Bracket bracket)
        {
            _brackets.Add(bracket);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var bracket = await _brackets.FindAsync(id);
            if (bracket == null)
            {
                throw new WebException("Bracket not found");
            }
            _brackets.Remove(bracket);
            await _context.SaveChangesAsync();
        }

        public async Task<Bracket> Get(int id)
        {
            return await _brackets.FindAsync(id);
        }

        public async Task<IEnumerable<Bracket>> GetAll()
        {
            return await _brackets.ToListAsync();
        }

        public async Task<IEnumerable<Bracket>> GetAllByTournamentId(int tournamentId)
        {
            return await _context.Brackets.Where(b => b.TournamentID == tournamentId).ToListAsync();
        }

        public async Task Update(Bracket bracket)
        {
            _brackets.Update(bracket);
            await _context.SaveChangesAsync();
        }
    }
}