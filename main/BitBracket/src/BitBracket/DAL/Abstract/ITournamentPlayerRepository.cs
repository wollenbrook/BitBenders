// using BitBracket.Models;
// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace BitBracket.DAL.Abstract
// {
//     public interface ITournamentPlayerRepository
//     {
//         Task<IEnumerable<Tournament>> GetAllTournamentsWithDetailsAsync();
//         Task<Tournament> GetTournamentByIdAsync(int tournamentId);
//         Task AddPlayerToTournamentAsync(JoinedPlayer player);
//         Task<IEnumerable<JoinedPlayer>> GetPlayersByTournamentIdAsync(int tournamentId);
//         Task RemovePlayerFromTournamentAsync(int playerId, int tournamentId);
//         Task SendJoinRequestAsync(SentJoinRequest request);
//         Task<IEnumerable<SentJoinRequest>> GetSentJoinRequestsByTournamentIdAsync(int tournamentId);
//         Task<IEnumerable<ReceivedPlayerRequest>> GetReceivedPlayerRequestsByTournamentIdAsync(int tournamentId);
//         Task ApproveJoinRequestAsync(int requestId);
//         Task DeclineJoinRequestAsync(int requestId);
//     }
// }
