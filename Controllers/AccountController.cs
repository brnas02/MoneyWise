using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Identity;

namespace DWebProjetoFinal.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Injeção de dependência do contexto da base de dados
        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // Lista todos os utilizadores (usado geralmente para o admin)
        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }

        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        // Retorna a view de registo
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                if (model.ProfileImage != null)
                {
                    const long MaxFileSize = 100 * 1024 * 1024;

                    if (model.ProfileImage.Length > 0 && model.ProfileImage.Length < MaxFileSize && IsValidImage(model.ProfileImage))
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
                    else
                    {
                        ModelState.AddModelError("", "Imagem Inválida");
                        return View(model);
                    }
                }

                var role = _context.Roles.FirstOrDefault(r => r.Type == model.Role);

                if (role == null || role.Type == "Admin")
                {
                    ModelState.AddModelError("", "Um erro inesperado ocorreu");
                    return View(model);
                }

                var hasher = new PasswordHasher<UserAccount>();

                // Cria o utilizador sem password
                UserAccount account = new UserAccount
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.UserName,
                    RoleId = role.Id,
                    ProfileImagePath = uniqueFileName != null ? "/uploads/" + uniqueFileName : null
                };

                try
                {
                    // Adiciona e guarda para obter o Id
                    _context.UserAccounts.Add(account);
                    await _context.SaveChangesAsync();

                    // Cria UserSeguranca com hash da password e associa ao utilizador
                    var hashedPassword = hasher.HashPassword(account, model.Password);
                    var userSeguranca = new UserSeguranca
                    {
                        UserId = account.Id,
                        Password = hashedPassword,
                        DataCriacao = DateTime.Now
                    };

                    _context.UserSeguranca.Add(userSeguranca);
                    await _context.SaveChangesAsync();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registado com sucesso";
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Coloque um Email ou Username válido");
                    return View(model);
                }

                return View();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Retorna a view de login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.UserAccounts
                .Include(u => u.UserSeguranca)
                .FirstOrDefault(x => x.UserName == model.UserNameOrEmail || x.Email == model.UserNameOrEmail);

            if (user != null && user.UserSeguranca != null)
            {
                var hasher = new PasswordHasher<UserAccount>();
                var result = hasher.VerifyHashedPassword(user, user.UserSeguranca.Password, model.Password);

                if (result == PasswordVerificationResult.Success)
                {
                    if (!user.IsActive)
                    {
                        ModelState.AddModelError("", "Conta desativada. Contacte o administrador.");
                        return View();
                    }

                    var role = _context.Roles.FirstOrDefault(r => r.Id == user.RoleId);

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Name", user.FirstName),
                new Claim(ClaimTypes.Role, role.Type),
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Dashboard", "Home");
                }
            }

            ModelState.AddModelError("", "Nome de utilizador/email ou palavra-passe incorretos");
            return View();
        }

        // Logout do utilizador
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Apresentacao", "Home");
        }

        // Retorna view de edição de perfil (dados atuais carregados)
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

        // Salva as alterações feitas ao perfil do utilizador
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

            // Atualiza os campos permitidos do utilizador autenticado
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

        // Acesso exclusivo a administradores: painel de admin
        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }

        // Lista todos os utilizadores para o administrador
        [Authorize(Roles = "Admin")]
        public IActionResult AdminUserList()
        {
            var users = _context.UserAccounts.ToList();
            return View(users);
        }

        // Alterna entre ativar e desativar conta de utilizador
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

        // Carrega os dados do utilizador a ser editado (admin)
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _context.UserAccounts.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View("EditUser", user); // Utiliza a view EditUser.cshtml
        }

        // Salva alterações feitas a um utilizador (admin)
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

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;
            user.UserName = updatedUser.UserName;

            _context.SaveChanges();

            return RedirectToAction("AdminUserList");
        }

        // Elimina utilizador (apenas por admin)
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

        private bool IsValidImage(IFormFile file)
        {
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var image = Image.FromStream(stream))
                    {
                        // Successfully loaded image
                        return true;
                    }
                }
            }
            catch
            {
                // Not a valid image
                return false;
            }
        }
    }
}