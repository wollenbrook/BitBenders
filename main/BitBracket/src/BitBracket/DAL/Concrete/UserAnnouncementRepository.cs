using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitBracket.DAL.Concrete
{
    public class UserAnnouncementRepository : IUserAnnouncementRepository
    {
        private readonly BitBracketDbContext _context;

        public UserAnnouncementRepository(BitBracketDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Announcement>> GetAllAsync()
        {
            return await _context.Announcements
                                 .Include(a => a.BitUser)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Announcement>> GetByUserIdAndStatus(int bitUserId, bool isDraft)
        {
            return await _context.Announcements
                                 .Where(a => a.BitUserId == bitUserId && a.IsDraft == isDraft)
                                 .Include(a => a.BitUser)
                                 .ToListAsync();
        }

        public async Task<Announcement> GetLatestAnnouncementAsync(int bitUserId)
        {
            return await _context.Announcements
                                 .Where(a => a.BitUserId == bitUserId)
                                 .OrderByDescending(a => a.CreationDate)
                                 .FirstOrDefaultAsync();
        }
    }
}


