using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IUserAnnouncementRepository
    {
        Task<IEnumerable<UserAnnouncement>> GetByUserIdAndStatus(string userId, bool isDraft);
        Task AddAsync(UserAnnouncement announcement);
        Task UpdateAsync(UserAnnouncement announcement);
        Task DeleteAsync(int id);
        Task<UserAnnouncement> GetByIdAsync(int id);
        Task<IEnumerable<UserAnnouncement>> GetPublishedAnnouncementsAsync();
    }
}
