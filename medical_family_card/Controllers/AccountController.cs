using medical_family_card.Models;
using medical_family_card.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;

namespace medical_family_card.Controllers
{
    public class AccountController : Controller
    {
        private MedicalFamilyCardDbContext db;

        public AccountController(MedicalFamilyCardDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";

            if (model.UsrEmail != null && !Regex.Match(model.UsrEmail, pattern, RegexOptions.IgnoreCase).Success)
            {
                ModelState.AddModelError("UsrEmail", "Неверный формат адреса почты");
            }
            if (model.UsrPassword != null && (model.UsrPassword.Length < 6 || model.UsrPassword.Length > 20))
            {
                ModelState.AddModelError("UsrPassword", "Допустимая длина пароля от 6 до 20");
            }

            if (ModelState.IsValid)
            {
                Usr user = await db.Usrs.FirstOrDefaultAsync(u => u.UsrEmail == model.UsrEmail && u.UsrPassword == model.UsrPassword);
                if (user != null)
                {
                    await Authenticate(Convert.ToString(user.UsrId)); // аутентификация
                    return RedirectToAction("Index", "Home", new { id = user.UsrId });
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            await Console.Out.WriteLineAsync(model.UsrEmail);

            string pattern = "[.\\-_a-z0-9]+@([a-z0-9][\\-a-z0-9]+\\.)+[a-z]{2,6}";

            if (model.UsrName != null && model.UsrName.Contains(' '))
            {
                ModelState.AddModelError("UsrName", "Имя пользователя не должно содержать пробелы");
            }
            if (model.UsrName != null && (model.UsrName.Length < 4 || model.UsrName.Length > 10))
            {
                ModelState.AddModelError("UsrName", "Допустимая длина имени пользователя от 4 до 10");
            }
            if (model.UsrEmail != null && !Regex.Match(model.UsrEmail, pattern, RegexOptions.IgnoreCase).Success)
            {
                ModelState.AddModelError("UsrEmail", "Неверный формат адреса почты");
            }
            if (model.UsrPassword != null && (model.UsrPassword.Length < 6 || model.UsrPassword.Length > 20))
            {
                ModelState.AddModelError("UsrPassword", "Допустимая длина пароля от 6 до 20");
            }
            if (model.UsrPassword != null && model.ConfirmUsrPassword != null && !model.UsrPassword.Equals(model.ConfirmUsrPassword))
            {
                ModelState.AddModelError("ConfirmUsrPassword", "Подтверждение пароля введено неверно");
            }

            if (ModelState.IsValid)
            {
                Usr email = await db.Usrs.FirstOrDefaultAsync(u => u.UsrEmail == model.UsrEmail);
                Usr name = await db.Usrs.FirstOrDefaultAsync(u => u.UsrName == model.UsrName);
                if (name == null && email == null)
                {
                    // добавляем пользователя в бд
                    db.Usrs.Add(new Usr { UsrEmail = model.UsrEmail, UsrPassword = model.UsrPassword, UsrName = model.UsrName });
                    await db.SaveChangesAsync();
                    Usr currentUser = await db.Usrs.FirstOrDefaultAsync(u => u.UsrEmail == model.UsrEmail);
                    await Authenticate(Convert.ToString(currentUser.UsrId)); // аутентификация
                    return RedirectToAction("Index", "Home", new { id = currentUser.UsrId });
                }
                if (email != null)
                    ModelState.AddModelError("UsrEmail", "Пользователь с такой почтой уже создан");
                if (name != null)
                    ModelState.AddModelError("UsrName", "Пользователь с таким именем уже создан");
            }
            return this.View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
