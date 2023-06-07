using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medical_family_card.Models;

namespace medical_family_card.Controllers
{
    public class AllergiesController : Controller
    {
        private readonly MedicalFamilyCardDbContext _context;

        public AllergiesController(MedicalFamilyCardDbContext context)
        {
            _context = context;
        }

        // GET: Allergies
        public async Task<IActionResult> Index(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == id);
            var medicalFamilyCardDbContext = _context.Allergies.Include(a => a.Usr);

            ViewData["UsrName"] = currentUser.UsrName;
            ViewData["currentUserId"] = id;

            return View(await medicalFamilyCardDbContext
                .Where(x => x.UsrId == id)
                .ToListAsync());
        }

        // GET: Allergies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Allergies == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies
                .Include(a => a.Usr)
                .FirstOrDefaultAsync(m => m.AllergyId == id);
            if (allergy == null)
            {
                return NotFound();
            }

            return View(allergy);
        }

        // GET: Allergies/Create
        public IActionResult Create()
        {
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId");
            return View();
        }

        // POST: Allergies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AllergyId,UsrId,AllergyName,AllergyComment")] Allergy allergy)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            allergy.Usr = currentUser;
            allergy.UsrId = currentUser.UsrId;

            if (allergy.AllergyName != null && allergy.AllergyName.Length > 20)
                ModelState.AddModelError("AllergyName", "Допустимая длина до 20 символов");

            if (allergy.AllergyComment != null && allergy.AllergyComment.Length > 200)
                ModelState.AddModelError("AllergyComment", "Допустимая длина до 200 символов");

            if (allergy.AllergyComment == null)
                allergy.AllergyComment = "";

            if (ModelState.IsValid)
            {
                _context.Add(allergy);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Allergies", new { id = currentUser.UsrId });
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", allergy.UsrId);
            return View(allergy);
        }

        // GET: Allergies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Allergies == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy == null)
            {
                return NotFound();
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", allergy.UsrId);
            return View(allergy);
        }

        // POST: Allergies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AllergyId,UsrId,AllergyName,AllergyComment")] Allergy allergy)
        {
            if (id != allergy.AllergyId)
            {
                return NotFound();
            }

            if (allergy.AllergyName != null && allergy.AllergyName.Length > 20)
                ModelState.AddModelError("AllergyName", "Допустимая длина до 20 символов");

            if (allergy.AllergyComment != null && allergy.AllergyComment.Length > 200)
                ModelState.AddModelError("AllergyComment", "Допустимая длина до 200 символов");

            if (allergy.AllergyComment == null)
                allergy.AllergyComment = "";

            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            allergy.Usr = currentUser;
            allergy.UsrId = currentUser.UsrId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(allergy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AllergyExists(allergy.AllergyId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Allergies", new { id = allergy.UsrId });
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", allergy.UsrId);
            return View(allergy);
        }

        // GET: Allergies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Allergies == null)
            {
                return NotFound();
            }

            var allergy = await _context.Allergies
                .Include(a => a.Usr)
                .FirstOrDefaultAsync(m => m.AllergyId == id);
            if (allergy == null)
            {
                return NotFound();
            }

            return View(allergy);
        }

        // POST: Allergies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Allergies == null)
            {
                return Problem("Entity set 'MedicalFamilyCardDbContext.Allergies'  is null.");
            }
            var allergy = await _context.Allergies.FindAsync(id);
            if (allergy != null)
            {
                _context.Allergies.Remove(allergy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Allergies", new { id = allergy.UsrId });
        }

        private bool AllergyExists(int id)
        {
          return (_context.Allergies?.Any(e => e.AllergyId == id)).GetValueOrDefault();
        }
    }
}
