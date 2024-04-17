using BitBracket.DAL.Abstract;
using BitBracket.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using EllipticCurve.Utils;
using BitBracket.DAL.Concrete;
using HW6.DAL.Concrete;

namespace BitBracket.DAL.Concrete
{
    public class GuidBracketRepository : Repository<GuidBracket>, IGuidBracketRepository
    {
        private readonly DbSet<GuidBracket> _guidbrackets;
        private readonly BitBracketDbContext _context;

        public GuidBracketRepository(BitBracketDbContext context) : base(context)
        {
           _context = context;
           _guidbrackets = context.GuidBrackets;
        }

        public async Task<GuidBracket> AddGuidBracket(GuidBracket guidBracket)
        {
            _guidbrackets.Add(guidBracket);
            await _context.SaveChangesAsync();
            return guidBracket;
        }

        public async Task<GuidBracket> DeleteGuidBracket(int id)
        {
            var guidBracket = await _guidbrackets.FindAsync(id);
            if (guidBracket == null)
            {
                throw new WebException("GuidBracket not found");
            }
            _guidbrackets.Remove(guidBracket);
            await _context.SaveChangesAsync();
            return guidBracket;
        }

        public async Task<GuidBracket> GetGuidBracket(Guid guid)
        {
            return await _guidbrackets.FirstOrDefaultAsync(e => e.Guid == guid);
        }

        public async Task<bool> GuidBracketExists(Guid guid)
        {
            return await _guidbrackets.AnyAsync(e => e.Guid== guid);
        }

        public async Task<GuidBracket> UpdateGuidBracket(GuidBracket guidBracket)
        {
            var existingGuidBracket = await _guidbrackets.FirstOrDefaultAsync(g => g.Guid == guidBracket.Guid) ?? throw new Exception("GuidBracket not found");
            existingGuidBracket.BracketData = guidBracket.BracketData;

            _context.GuidBrackets.Update(existingGuidBracket);
            await _context.SaveChangesAsync();

            return existingGuidBracket;
        }
       
    }

}