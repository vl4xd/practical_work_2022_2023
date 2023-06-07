using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medical_family_card.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using NuGet.Versioning;

namespace medical_family_card.Controllers
{
    public class WeightController : Controller
    {
        private readonly MedicalFamilyCardDbContext _context;

        public WeightController(MedicalFamilyCardDbContext context)
        {
            _context = context;
        }

        // GET: Weight
        public async Task<IActionResult> Index(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == id);
            var medicalFamilyCardDbContext = _context.Wts.Include(w => w.Usr);

            ViewData["UsrName"] = currentUser.UsrName;
            ViewData["currentUserId"] = id;

            return View(await medicalFamilyCardDbContext
                .Where(x => x.UsrId == id)
                .ToListAsync());
        }

        // GET: Weight/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wts == null)
            {
                return NotFound();
            }

            var wt = await _context.Wts
                .Include(w => w.Usr)
                .FirstOrDefaultAsync(m => m.WtId == id);
            if (wt == null)
            {
                return NotFound();
            }

            return View(wt);
        }

        // GET: Weight/Create
        public IActionResult Create()
        {
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId");

            ViewBag.CurrentUserId = Convert.ToInt64(User.Identity.Name);

            return View();
        }

        // POST: Weight/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WtId,UsrId,WtValue,WtDate")] Wt wt, double WtValue)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            wt.Usr = currentUser;
            wt.UsrId = currentUser.UsrId;

            if (wt.WtValue != null && (wt.WtValue > 610 || wt.WtValue < 1))
                ModelState.AddModelError("WtValue", "Допустимая величина от 1 до 610 кг");

            if (wt.WtDate != null && wt.WtDate > DateTime.Now)
                ModelState.AddModelError("WtDate", "Введена недопустимая дата изменерия");

            if (ModelState.IsValid)
            {
                _context.Add(wt);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Weight", new { id = currentUser.UsrId });
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", wt.UsrId);

            ViewBag.CurrentUserId = wt.UsrId;

            return View(wt);
        }

        // GET: Weight/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wts == null)
            {
                return NotFound();
            }

            var wt = await _context.Wts.FindAsync(id);
            if (wt == null)
            {
                return NotFound();
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", wt.UsrId);
            return View(wt);
        }

        // POST: Weight/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WtId,UsrId,WtValue,WtDate")] Wt wt)
        {
            int currentUserId;
            if (id != wt.WtId)
            {
                return NotFound();
            }

            if (wt.WtValue != null && (wt.WtValue > 610 || wt.WtValue < 1))
                ModelState.AddModelError("WtValue", "Допустимая величина от 1 до 610 кг");

            if (wt.WtDate != null && wt.WtDate > DateTime.Now)
                ModelState.AddModelError("WtDate", "Введена недопустимая дата изменерия");

            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt32(User.Identity.Name));
            wt.Usr = currentUser;
            wt.UsrId = currentUser.UsrId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WtExists(wt.WtId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Weight", new { id = wt.UsrId });
            }
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", wt.UsrId);

            ViewBag.CurrentUserId = wt.UsrId;

            return View(wt);
        }

        // GET: Weight/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wts == null)
            {
                return NotFound();
            }

            var wt = await _context.Wts
                .Include(w => w.Usr)
                .FirstOrDefaultAsync(m => m.WtId == id);
            if (wt == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUserId = wt.UsrId;

            return View(wt);
        }

        // POST: Weight/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int currentUserId;
            if (_context.Wts == null)
            {
                return Problem("Entity set 'MedicalFamilyCardDbContext.Wts'  is null.");
            }
            var wt = await _context.Wts.FindAsync(id);
            currentUserId = wt.UsrId;
            if (wt != null)
            {
                _context.Wts.Remove(wt);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Weight", new { id = wt.UsrId });
        }

        private bool WtExists(int id)
        {
          return (_context.Wts?.Any(e => e.WtId == id)).GetValueOrDefault();
        }
    }
}
