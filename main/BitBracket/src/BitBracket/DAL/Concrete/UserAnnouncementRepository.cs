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

        public async Task AddAsync(UserAnnouncement announcement)
        {
            _context.UserAnnouncements.Add(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var announcement = await _context.UserAnnouncements.FindAsync(id);
            if (announcement != null)
            {
                _context.UserAnnouncements.Remove(announcement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserAnnouncement> GetByIdAsync(int id)
        {
            return await _context.UserAnnouncements
                                 .Include(a => a.BitUser)
                                 .Include(a => a.Tournament)
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<UserAnnouncement>> GetByUserIdAndStatus(string userId, bool isDraft)
        {
            return await _context.UserAnnouncements
                                .Include(a => a.Tournament)
                                .Where(a => a.Owner.ToString() == userId && a.IsDraft == isDraft)
                                .ToListAsync();
        }


        public async Task UpdateAsync(UserAnnouncement announcement)
        {
            _context.UserAnnouncements.Update(announcement);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserAnnouncement>> GetPublishedAnnouncementsAsync()
        {
            return await _context.UserAnnouncements
                .Include(ua => ua.Tournament) // Include the Tournament entity
                .Where(ua => !ua.IsDraft)
                .ToListAsync();
        }

    }
}

