using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BitBracket.DAL.Concrete
{
    public class BitUserRepository : IBitUserRepository
    {
        private readonly BitBracketDbContext _context;

        public BitUserRepository(BitBracketDbContext context)
        {
            _context = context;
        }

        public BitUser GetBitUserByEntityId(string id)
        {
            return _context.Set<BitUser>().FirstOrDefault(u => u.AspnetIdentityId == id);
        }


    }
    
}


