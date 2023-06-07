using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using medical_family_card.Models;
using NuGet.Packaging.Signing;
using medical_family_card.ViewModels;

namespace medical_family_card.Controllers
{
    public class FriendsController : Controller
    {
        private readonly MedicalFamilyCardDbContext _context;

        public FriendsController(MedicalFamilyCardDbContext context)
        {
            _context = context;
        }

        // GET: Friends
        public async Task<IActionResult> Index()
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            var medicalFamilyCardDbContext = _context.Friends.Include(f => f.FriendType).Include(f => f.FromUsr).Include(f => f.ToUsr);
            return View(await medicalFamilyCardDbContext
                .Where(f => 
                ((f.FromUsrId != currentUser.UsrId && f.ToUsrId == currentUser.UsrId) || 
                (f.FromUsrId == currentUser.UsrId && f.ToUsrId != currentUser.UsrId)) && 
                f.FriendTypeId == 1)
                .ToListAsync());
        }

        // GET: Friends/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Friends == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends
                .Include(f => f.FriendType)
                .Include(f => f.FromUsr)
                .Include(f => f.ToUsr)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));
            if (currentUser.UsrId == friend.FromUsrId)
            {
                return Redirect($"/Home/Index/{friend.ToUsrId}");
            }
            else
            {
                return Redirect($"/Home/Index/{friend.FromUsrId}");
            }
        }

        // GET: Friends/Create
        public IActionResult Create()
        {
            //ViewData["FriendTypeId"] = new SelectList(_context.FriendTypes, "FriendTypeId", "FriendTypeName");
            //ViewData["FromUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName");
            //ViewData["ToUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName");
            return View();
        }

        // POST: Friends/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("FriendId,FromUsrId,ToUsrId,FriendTypeId")] Friend friend)
        //{
        //    Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));
        //    friend.FromUsr = currentUser;
        //    friend.FriendTypeId = 2;

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(friend);
        //        await _context.SaveChangesAsync();
        //        return Redirect($"/Friends/Index/{currentUser.UsrId}");
        //    }
        //    ViewData["FriendTypeId"] = new SelectList(_context.FriendTypes, "FriendTypeId", "FriendTypeName", friend.FriendTypeId);
        //    ViewData["FromUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName", friend.FromUsrId);
        //    ViewData["ToUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName", friend.ToUsrId);
        //    return View(friend);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FindFriendModel model)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            if (model.UserNameTo != null && model.UserNameTo.Contains(' '))
                ModelState.AddModelError("UserNameTo", "Имя пользователя не должно содержать пробелы");

            if (model.UserNameTo != null && (model.UserNameTo.Length < 4 || model.UserNameTo.Length > 10))
                ModelState.AddModelError("UserNameTo", "Допустимая длина имени пользователя от 4 до 10");

            if (model.UserNameTo.Equals(currentUser.UsrName))
                ModelState.AddModelError("UserNameTo", "Вы ввели себя");

            

            if (ModelState.IsValid)
            {
                Usr userTo = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrName == model.UserNameTo);

                Friend friend = await _context.Friends
                    .FirstOrDefaultAsync(f =>
                    (f.ToUsrId == userTo.UsrId && f.FromUsrId == currentUser.UsrId) ||
                    (f.ToUsrId == currentUser.UsrId && f.FromUsrId == userTo.UsrId));

                if (friend == null)
                {
                    _context.Add(new Friend {
                        FromUsrId = currentUser.UsrId,
                        ToUsrId = userTo.UsrId,
                        FriendTypeId = 2,
                        FromUsr = currentUser,
                        ToUsr = userTo
                    });
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Friends", new { id = currentUser.UsrId });
                }
                else
                    ModelState.AddModelError("UserNameTo", "Знакомый уже находится в вашем списке");
            }
            //ViewData["FriendTypeId"] = new SelectList(_context.FriendTypes, "FriendTypeId", "FriendTypeName", friend.FriendTypeId);
            //ViewData["FromUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName", friend.FromUsrId);
            //ViewData["ToUsrId"] = new SelectList(_context.Usrs, "UsrId", "UsrName", friend.ToUsrId);
            return View(model);
        }

        // GET: Friends/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Friends == null)
            {
                return NotFound();
            }

            var friend = await _context.Friends
                .Include(f => f.FriendType)
                .Include(f => f.FromUsr)
                .Include(f => f.ToUsr)
                .FirstOrDefaultAsync(m => m.FriendId == id);
            if (friend == null)
            {
                return NotFound();
            }

            return View(friend);
        }

        // POST: Friends/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            if (_context.Friends == null)
            {
                return Problem("Entity set 'MedicalFamilyCardDbContext.Friends'  is null.");
            }
            var friend = await _context.Friends.FindAsync(id);
            if (friend != null)
            {
                _context.Friends.Remove(friend);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Friends", new { id = currentUser.UsrId });
        }

        public async Task<IActionResult> RequestFrom(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            var medicalFamilyCardDbContext = _context.Friends.Include(f => f.FriendType).Include(f => f.FromUsr).Include(f => f.ToUsr);
            return View(await medicalFamilyCardDbContext
                .Where(f => f.FromUsrId == currentUser.UsrId && f.FriendTypeId == 2)
                .ToListAsync());
        }

        public async Task<IActionResult> RequestTo(int id)
        {
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            var medicalFamilyCardDbContext = _context.Friends.Include(f => f.FriendType).Include(f => f.FromUsr).Include(f => f.ToUsr);
            return View(await medicalFamilyCardDbContext
                .Where(f => f.ToUsrId == currentUser.UsrId && f.FriendTypeId == 2)
                .ToListAsync());
        }

        public async Task<IActionResult> FriendAccept(int id)
        {
            Friend friend = await _context.Friends.FirstOrDefaultAsync(u => u.FriendId == id);
            Usr currentUser = await _context.Usrs.FirstOrDefaultAsync(u => u.UsrId == Convert.ToInt64(User.Identity.Name));

            if (friend == null)
            {
                return NotFound();
            }

            friend.FriendTypeId = 1;

            try
            {
                _context.Update(friend);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(friend.FriendId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("RequestTo", "Friends", new { id = currentUser.UsrId });
        }

        private bool FriendExists(int id)
        {
          return (_context.Friends?.Any(e => e.FriendId == id)).GetValueOrDefault();
        }
    }
}
