using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;

namespace DWebProjetoFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.ProfileImage.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ProfileImage.CopyToAsync(stream);
                    }
                }

                UserAccount account = new UserAccount
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    UserName = model.UserName,
                    Role = model.Role,
                    ProfileImagePath = uniqueFileName != null ? "/uploads/" + uniqueFileName : null
                };

                try
                {
                    _context.UserAccounts.Add(account);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully. Please login";
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password");
                    return View(model);
                }

                return View();
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.FirstOrDefault(x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail) && x.Password == model.Password);
                if (user != null)
                {
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", "Conta desativada. Contacte o administrador.");
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("Name", user.FirstName),
                        new Claim(ClaimTypes.Role, user.Role),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }
            return View();
        }

        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Apresentacao", "Home");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserAccount updatedUser)
        {
            Console.WriteLine("Chamou o POST Edit");

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var user = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                user,
                "",
                u => u.FirstName, u => u.LastName, u => u.Email, u => u.UserName))
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            Console.WriteLine("ModelState inválido");
            return View(updatedUser);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminUserList()
        {
            var users = _context.UserAccounts.ToList();
            return View(users);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult ToggleUserStatus(int id)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.IsActive = !user.IsActive;
            _context.SaveChanges();

            return RedirectToAction("AdminUserList");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("EditUser", user); // Vai usar uma view chamada EditUser.cshtml
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(UserAccount updatedUser)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == updatedUser.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Atualiza os campos permitidos
            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.UserName = updatedUser.UserName;

            _context.SaveChanges();

            return RedirectToAction("AdminUserList");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.UserAccounts.Remove(user);
                _context.SaveChanges();
            }

            return RedirectToAction("AdminUserList");
        }
    }
}
