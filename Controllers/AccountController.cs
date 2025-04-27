using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;

namespace DWebProjetoFinal.Controllers {
    public class AccountController : Controller {

        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext){
            _context = appDbContext;
        }
        public IActionResult Index() {
            return View(_context.UserAccounts.ToList());
        }

        public IActionResult Registration() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // Verifica se o utilizador fez upload da imagem
                if (model.ProfileImage != null && model.ProfileImage.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    Directory.CreateDirectory(uploadsFolder); // Garante que a pasta existe

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
                    Type = model.Type,
                    ProfileImagePath = uniqueFileName != null ? "/uploads/" + uniqueFileName : null
                };

                try
                {
                    _context.UserAccounts.Add(account);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully. Please login";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email or Password");
                    return View(model);
                }

                return View();
            }

            return View(model);
        }

        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model) {
            if (ModelState.IsValid){
                var user = _context.UserAccounts.Where(x => (x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail) && x.Password == model.Password).FirstOrDefault();
                if (user != null) {
                    //Success, create cookie
                    var claims = new List<Claim>{
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim("Name", user.FirstName),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("SecurePage");
                } else {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                }
            }
            return View();
        }

        public IActionResult LogOut() { 
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home"); 
        }

        [Authorize]
        public IActionResult SecurePage(){
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
