//DAL/Abstract/ITournamentRepository.cs

using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BitBracket.DAL.Abstract
{
    public interface ITournamentRepository
    {
        Task<Tournament> Get(int id);
        Task<IEnumerable<Tournament>> GetAll();
        Task<IEnumerable<Tournament>> GetAllByOwnerId(int ownerId);
        Task Add(Tournament tournament);
        Task Update(Tournament tournament);
        Task Delete(int id);
        //
        Task<bool> SendParticipateRequest(int userId, int tournamentId);
        Task<bool> AcceptParticipateRequest(int requestId);
        Task<bool> DeclineParticipateRequest(int requestId);
        Task<bool> RemoveParticipate(int userId, int tournamentId);
        Task<bool> CheckIfParticipates(int userId, int tournamentId);
        Task<IEnumerable<ParticipateRequest>> GetParticipateRequests(int tournamentId);
        Task<IEnumerable<Participate>> GetParticipates(int tournamentId);
        //
        Task<IEnumerable<Tournament>> GetTournamentsByUserId(int userId);
        Task<bool> WithdrawFromTournament(int userId, int tournamentId);
    }
}