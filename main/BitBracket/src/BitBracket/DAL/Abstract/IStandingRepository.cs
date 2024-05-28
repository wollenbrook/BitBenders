using BitBracket.DAL.Concrete;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface IStandingRepository : IRepository<Standing>
    {
        public Task<List<Standing>> GetByPersonId(int id);
        public Task InsertOrUpdatePlacement(int bitUserId, int tournamentId, int placement);

    }
}
