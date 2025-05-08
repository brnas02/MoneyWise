using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models; 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    public class TransacaoController : Controller {
        private readonly AppDbContext _context;

        public TransacaoController(AppDbContext context) {
            _context = context;
        }

        // GET: Transaction/NewTransaction
        [Authorize]
        public IActionResult NovaTransacao() {
            return View();
        }

        // POST: Transaction/NewTransaction
        [HttpPost]
        [Authorize]
        public IActionResult NovaTransacao(Transacao model) {
            if (ModelState.IsValid) {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim)) {
                    return Unauthorized();
                }

                model.UserId = int.Parse(userIdClaim); // Liga a transação ao utilizador autenticado

                _context.Transactions.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home"); // Ou voltar para lista de transações
            }

            return View(model);
        }
    }
}
