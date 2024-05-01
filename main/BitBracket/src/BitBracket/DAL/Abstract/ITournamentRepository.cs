using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface ITournamentRepository
    {
        Task<Tournament> Get(int id);
        Task<IEnumerable<Tournament>> GetByName(string Name);
        Task<IEnumerable<Tournament>> GetAll();
        Task<IEnumerable<Tournament>> GetAllByOwnerId(int ownerId);
        Task Add(Tournament tournament);
        Task Update(Tournament tournament);
        Task Delete(int id);
    }
}