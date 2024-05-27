using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using NuGet.Protocol.Plugins;
using System.Reflection;
using Twilio.TwiML.Fax;

namespace BitBracket.DAL.Concrete
{
    public class StandingRepository : Repository<Standing>, IStandingRepository
    {
        private readonly DbSet<Standing> _standings;
        private readonly BitBracketDbContext _context;
        private readonly DbSet<BitUser> _bitUser;

        public StandingRepository(BitBracketDbContext context) : base(context)
        {
            _standings = context.Standings;
            _context = context;
            _bitUser = context.BitUsers;
        }

        public Task<List<Standing>> GetByPersonId(int personId)
        {
            return _standings.Where(s => s.Person == personId).ToListAsync();
        }
        public Task InsertOrUpdatePlacement(int personId, int tournamentId, int placement)
        {
            var standing = _standings.FirstOrDefault(s => s.Person == personId && s.TournamentId == tournamentId);
            if (standing == null)
            {
                standing = new Standing
                {
                    Person = personId,
                    TournamentId = tournamentId,
                    Placement = placement
                };
                _standings.Add(standing);
                BitUser bitUser = _bitUser.FirstOrDefault(u => u.Id == personId);
                bitUser.Standings.Add(standing);
            }
            else
            {
                standing.Placement = placement;
            }
            return _context.SaveChangesAsync();
        }

    }
    
}
