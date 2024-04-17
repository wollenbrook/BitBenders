using System.Collections.Generic;
using System.Threading.Tasks;
using BitBracket.Models;

namespace BitBracket.DAL.Abstract
{
    public interface IGuidBracketRepository
    {
        Task<GuidBracket> GetGuidBracket(Guid guid);
        Task<GuidBracket> AddGuidBracket(GuidBracket guidBracket);
        Task<GuidBracket> UpdateGuidBracket(GuidBracket guidBracket);
        Task<GuidBracket> DeleteGuidBracket(int id);
        Task<bool> GuidBracketExists(Guid guid);
    }
}