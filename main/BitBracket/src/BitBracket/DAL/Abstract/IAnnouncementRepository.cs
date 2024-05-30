using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IAnnouncementRepository
    {
        Task<IEnumerable<Announcement>> GetAllAsync();
        Task AddAsync(Announcement announcement);
        Task<Announcement> GetLatestAnnouncementAsync();
    }
}
