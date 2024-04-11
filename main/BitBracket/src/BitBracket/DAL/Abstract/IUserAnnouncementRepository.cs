using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IUserAnnouncementRepository
    {
        Task AddAsync(Announcement announcement);
        Task<IEnumerable<Announcement>> GetByUserIdAndStatus(int bitUserId, bool isDraft);
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task<Announcement> GetLatestAnnouncementAsync(int bitUserId);
    }
}


