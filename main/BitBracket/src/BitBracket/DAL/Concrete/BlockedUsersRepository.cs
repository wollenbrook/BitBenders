using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using BitBracket.DAL.Concrete;
using HW6.DAL.Concrete;
using NuGet.Protocol.Plugins;
using System.Reflection;
using Twilio.TwiML.Fax;
using BitBracket.Models;
using System.Collections.Generic;
using BitBracket.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Collections;


namespace BitBracket.DAL.Concrete
{
    public class BlockedUsersRepository : Repository<BlockedUser>, IBlockedUsersRepository
    {
        private readonly DbSet<BlockedUser> _blockedUsers;
        private readonly BitBracketDbContext _context;


        public BlockedUsersRepository(BitBracketDbContext context) : base(context)
    {
            _blockedUsers = context.BlockedUsers;
            _context = context;
        }

        public Task<IEnumerable<BlockedUser>> GetAllBlockedUsers()
        {
            IEnumerable<BlockedUser> blockedList = _blockedUsers.ToList();
            return Task.FromResult(blockedList);
        }
    }

}
