//DAL/Abstract/ITournamentRepository.cs

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
        //
        Task<bool> SendParticipationRequestAsync(int userId, int tournamentId);
        Task<bool> AcceptParticipationRequestAsync(int requestId);
        Task<bool> DenyOrRemoveParticipationRequestAsync(int requestId);
        Task<bool> RemoveParticipantAsync(int userId, int tournamentId);
        Task<bool> CheckIfParticipatesAsync(int userId, int tournamentId);
        Task<IEnumerable<Tournament>> GetTournamentsUserParticipatesInAsync(int userId);
        Task<bool> WithdrawFromTournamentAsync(int userId, int tournamentId);
        Task<IEnumerable<ParticipateRequest>> GetAllParticipationRequests(int tournamentId);
        Task<IEnumerable<Participate>> GetApprovedParticipates(int tournamentId);
    }
}