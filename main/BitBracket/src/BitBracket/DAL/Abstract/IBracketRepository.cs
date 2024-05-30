using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IBracketRepository
    {
        Task Add(Bracket bracket);
        Task Delete(int id);
        Task<Bracket> Get(int id);
        Task<IEnumerable<Bracket>> GetAll();
        Task<IEnumerable<Bracket>> GetAllByTournamentId(int tournamentId);
        Task Update(Bracket bracket);
    }
}