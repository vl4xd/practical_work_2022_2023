using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medical_family_card.Models;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace medical_family_card.Controllers
{
    public class UsrInfoesController : Controller
    {
        private readonly MedicalFamilyCardDbContext _context;

        public UsrInfoesController(MedicalFamilyCardDbContext context)
        {
            _context = context;
        }

        // GET: UsrInfoes
        public async Task<IActionResult> Index(int id)
        {
            var medicalFamilyCardDbContext = _context.UsrInfos.Include(u => u.Blood).Include(u => u.Gender).Include(u => u.RhesusFactor).Include(u => u.Usr);
            return Redirect($"/UsrInfoes/Details/{id}");
            //return View(await medicalFamilyCardDbContext.ToListAsync());
        }

        public async Task<IActionResult> IsNotExist(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == id);

            if (currentUser.UsrId == Convert.ToInt64(User.Identity.Name))
            {
                ViewBag.MainUser = true;
                ViewBag.UserName = "Ваша";
            }
            else
            {
                ViewBag.MainUser = false;
                ViewBag.UserName = currentUser.UsrName;
            }

            return View(id);
        }

        // GET: UsrInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsrInfos == null)
            {
                return NotFound();
            }

            var usrInfo = await _context.UsrInfos
                .Include(u => u.Blood)
                .Include(u => u.Gender)
                .Include(u => u.RhesusFactor)
                .Include(u => u.Usr)
                .FirstOrDefaultAsync(m => m.UsrId == id);
            if (usrInfo == null)
            {
                return Redirect($"/UsrInfoes/IsNotExist/{id}");
            }

            return View(usrInfo);
        }

        // GET: UsrInfoes/Create
        public IActionResult Create()
        {
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName");
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderName");
            ViewData["RhesusFactorId"] = new SelectList(_context.RhesusFactors, "RhesusFactorId", "RhesusFactorName");
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId");
            return View();
        }

        // POST: UsrInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsrId,UsrInfoFirstName,UsrInfoLastName,UsrInfoMiddleName,UsrInfoBirthday,GenderId,BloodId,RhesusFactorId")] UsrInfo usrInfo,
            IFormFile? uploadImage)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));
            usrInfo.Usr = currentUser;
            usrInfo.UsrId = currentUser.UsrId;

            byte[] imageData = null;
            using (var fileStream = new FileStream("..\\medical_family_card\\Images\\IconDefaultUserPhoto.png", FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    imageData = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }
            usrInfo.ImgName = "IconDefaultUserPhoto.png";
            usrInfo.ImgData = imageData;

            if (ModelState.IsValid)
            {
                if (uploadImage != null)
                {
                    imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)uploadImage.Length);
                    }
                    usrInfo.ImgName = uploadImage.FileName;
                    usrInfo.ImgData = imageData;
                }
                _context.Add(usrInfo);
                await _context.SaveChangesAsync();
                return Redirect($"/UsrInfoes/Index/{currentUser.UsrId}");
            }
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", usrInfo.BloodId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderName", usrInfo.GenderId);
            ViewData["RhesusFactorId"] = new SelectList(_context.RhesusFactors, "RhesusFactorId", "RhesusFactorName", usrInfo.RhesusFactorId);
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", usrInfo.UsrId);
            return View(usrInfo);
        }

        // GET: UsrInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsrInfos == null)
            {
                return NotFound();
            }

            var usrInfo = await _context.UsrInfos.FindAsync(id);
            if (usrInfo == null)
            {
                return NotFound();
            }
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", usrInfo.BloodId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderName", usrInfo.GenderId);
            ViewData["RhesusFactorId"] = new SelectList(_context.RhesusFactors, "RhesusFactorId", "RhesusFactorName", usrInfo.RhesusFactorId);
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", usrInfo.UsrId);
            return View(usrInfo);
        }

        // POST: UsrInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsrId,UsrInfoFirstName,UsrInfoLastName,UsrInfoMiddleName,UsrInfoBirthday,GenderId,BloodId,RhesusFactorId")] UsrInfo usrInfo,
            IFormFile? uploadImage)
        {
            if (id != usrInfo.UsrId)
            {
                return NotFound();
            }

            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));
            usrInfo.Usr = currentUser;
            usrInfo.UsrId = currentUser.UsrId;

            UsrInfo currentUserInfo = await _context.UsrInfos.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            byte[] imageData = null;
            using (var fileStream = new FileStream("..\\medical_family_card\\Images\\IconDefaultUserPhoto.png", FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    imageData = binaryReader.ReadBytes((int)fileStream.Length);
                }
            }
            usrInfo.ImgName = "IconDefaultUserPhoto.png";
            usrInfo.ImgData = imageData;

            if (ModelState.IsValid)
            {
                try
                {
                    if (uploadImage != null)
                    {
                        imageData = null;
                        using (var binaryReader = new BinaryReader(uploadImage.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)uploadImage.Length);
                        }
                        usrInfo.ImgName = uploadImage.FileName;
                        usrInfo.ImgData = imageData;
                    }
                    else
                    {
                        usrInfo.ImgName = currentUserInfo.ImgName;
                        usrInfo.ImgData = currentUserInfo.ImgData;
                    }
                    //_context.Update(usrInfo);
                    //_context.Remove(usrInfo);
                    //_context.Add(usrInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsrInfoExists(usrInfo.UsrId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect($"/UsrInfoes/Index/{usrInfo.UsrId}");
            }
            ViewData["BloodId"] = new SelectList(_context.Bloods, "BloodId", "BloodName", usrInfo.BloodId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "GenderId", "GenderName", usrInfo.GenderId);
            ViewData["RhesusFactorId"] = new SelectList(_context.RhesusFactors, "RhesusFactorId", "RhesusFactorName", usrInfo.RhesusFactorId);
            ViewData["UsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrId", usrInfo.UsrId);
            return View(usrInfo);
        }

        // GET: UsrInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsrInfos == null)
            {
                return NotFound();
            }

            var usrInfo = await _context.UsrInfos
                .Include(u => u.Blood)
                .Include(u => u.Gender)
                .Include(u => u.RhesusFactor)
                .Include(u => u.Usr)
                .FirstOrDefaultAsync(m => m.UsrId == id);
            if (usrInfo == null)
            {
                return NotFound();
            }

            return View(usrInfo);
        }

        // POST: UsrInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsrInfos == null)
            {
                return Problem("Entity set 'MedicalFamilyCardDbContext.UsrInfos'  is null.");
            }
            var usrInfo = await _context.UsrInfos.FindAsync(id);
            if (usrInfo != null)
            {
                _context.UsrInfos.Remove(usrInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("IsNotExist", "UsrInfoes", new { id = usrInfo.UsrId });
        }

        private bool UsrInfoExists(int id)
        {
          return (_context.UsrInfos?.Any(e => e.UsrId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> GetImage(int id)
        {
            UsrInfo currentUserInfo = await _context.UsrInfos.FirstOrDefaultAsync(u => u.UsrId == id);
            return File(currentUserInfo.ImgData, "image/png");
        }
    }
}
