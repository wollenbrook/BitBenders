using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using BitBracket.DAL.Concrete;
using NuGet.Protocol.Plugins;
using System.Reflection;
using Twilio.TwiML.Fax;

namespace BitBracket.DAL.Abstract
{
    public interface IBlockedUsersRepository : IRepository<BlockedUser>
    {
        public Task<IEnumerable<BlockedUser>> GetAllBlockedUsers();


    }
}
