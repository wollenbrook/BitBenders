using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BitBracket.Models;

namespace BitBracket.Controllers
{
    public class BitUsersController : Controller
    {
        private readonly BitBracketDbContext _context;

        public BitUsersController(BitBracketDbContext context)
        {
            _context = context;
        }

        // GET: BitUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.BitUser.ToListAsync());
        }

        // GET: BitUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bitUser = await _context.BitUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bitUser == null)
            {
                return NotFound();
            }

            return View(bitUser);
        }

        // GET: BitUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BitUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AspnetIdentityId,Username,Tag")] BitUser bitUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bitUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bitUser);
        }

        // GET: BitUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bitUser = await _context.BitUser.FindAsync(id);
            if (bitUser == null)
            {
                return NotFound();
            }
            return View(bitUser);
        }

        // POST: BitUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AspnetIdentityId,Username,Tag")] BitUser bitUser)
        {
            if (id != bitUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bitUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BitUserExists(bitUser.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bitUser);
        }

        // GET: BitUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bitUser = await _context.BitUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bitUser == null)
            {
                return NotFound();
            }

            return View(bitUser);
        }

        // POST: BitUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bitUser = await _context.BitUser.FindAsync(id);
            if (bitUser != null)
            {
                _context.BitUser.Remove(bitUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BitUserExists(int id)
        {
            return _context.BitUser.Any(e => e.Id == id);
        }
    }
}
