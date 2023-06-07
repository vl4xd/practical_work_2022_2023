using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medical_family_card.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace medical_family_card.Controllers
{
    public class HeightController : Controller
    {
        private readonly MedicalFamilyCardDbContext _context;

        public HeightController(MedicalFamilyCardDbContext context)
        {
            _context = context;
        }

        // GET: Height
        public async Task<IActionResult> Index(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == id);
            var medicalFamilyCardDbContext = _context.Hts.Include(h => h.Usr);

            ViewData["UsrName"] = currentUser.UsrName;
            ViewData["currentUserId"] = id;

            return View(await medicalFamilyCardDbContext
                .Where(x => x.UsrId == id)
                .ToListAsync());
        }

        // GET: Height/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hts == null)
            {
                return NotFound();
            }

            var ht = await _context.Hts
                .Include(h => h.Usr)
                .FirstOrDefaultAsync(m => m.HtId == id);
            if (ht == null)
            {
                return NotFound();
            }

            return View(ht);
        }

        // GET: Height/Create
        public IActionResult Create()
        {
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId");
            return View();
        }

        // POST: Height/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HtId,UsrId,HtValue,HtDate")] Ht ht)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            ht.Usr = currentUser;
            ht.UsrId = currentUser.UsrId;

            if (ht.HtValue != null && (ht.HtValue > 251 || ht.HtValue < 30))
                ModelState.AddModelError("HtValue", "Допустимая величина от 30 до 251 см");

            if (ht.HtDate != null && ht.HtDate > DateTime.Now)
                ModelState.AddModelError("HtDate", "-Док, а как же все эти разговоры, что нельзя менять будущее?");

            if (ModelState.IsValid)
            {
                _context.Add(ht);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Height", new { id = currentUser.UsrId});
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", ht.UsrId);
            return View(ht);
        }

        // GET: Height/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hts == null)
            {
                return NotFound();
            }

            var ht = await _context.Hts.FindAsync(id);
            if (ht == null)
            {
                return NotFound();
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", ht.UsrId);
            return View(ht);
        }

        // POST: Height/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HtId,UsrId,HtValue,HtDate")] Ht ht)
        {
            if (id != ht.HtId)
            {
                return NotFound();
            }

            if (ht.HtValue != null && (ht.HtValue > 251 || ht.HtValue < 30))
                ModelState.AddModelError("HtValue", "Допустимая величина от 30 до 251 см");

            if (ht.HtDate != null && ht.HtDate > DateTime.Now)
                ModelState.AddModelError("HtDate", "Введена недопустимая дата изменерия");

            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            ht.Usr = currentUser;
            ht.UsrId = currentUser.UsrId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ht);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HtExists(ht.HtId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Height", new { id = ht.UsrId });
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", ht.UsrId);
            return View(ht);
        }

        // GET: Height/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hts == null)
            {
                return NotFound();
            }

            var ht = await _context.Hts
                .Include(h => h.Usr)
                .FirstOrDefaultAsync(m => m.HtId == id);
            if (ht == null)
            {
                return NotFound();
            }

            return View(ht);
        }

        // POST: Height/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hts == null)
            {
                return Problem("Entity set 'MedicalFamilyCardDbContext.Hts'  is null.");
            }
            var ht = await _context.Hts.FindAsync(id);
            if (ht != null)
            {
                _context.Hts.Remove(ht);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Height", new { id = ht.UsrId });
        }

        private bool HtExists(int id)
        {
          return (_context.Hts?.Any(e => e.HtId == id)).GetValueOrDefault();
        }
    }
}
