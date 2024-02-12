using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.DAL.Concrete
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly BitBracketDbContext _context;

        public AnnouncementRepository(BitBracketDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _context.Announcements.ToListAsync();
        }

        public async Task AddAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }
    }
}
