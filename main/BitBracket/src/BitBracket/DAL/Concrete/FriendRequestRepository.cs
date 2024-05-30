using BitBracket.DAL.Abstract;
using BitBracket.Models;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.DAL.Concrete
{
    public class FriendRequestRepository : Repository<FriendRequest>, IFriendRequestRepository
    {
        private readonly DbSet<FriendRequest> _friendRequests;
        public FriendRequestRepository(BitBracketDbContext context) : base(context)
        {
            _friendRequests = context.FriendRequests;
           
        }
    }
}
