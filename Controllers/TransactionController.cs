using DWebProjetoFinal.Entities; // Para aceder à entidade Transaction
using DWebProjetoFinal.Models;    // (se usarmos ViewModel)
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    public class TransactionController : Controller {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context) {
            _context = context;
        }

        // GET: Transaction/NewTransaction
        [Authorize]
        public IActionResult NewTransaction() {
            return View();
        }

        // POST: Transaction/NewTransaction
        [HttpPost]
        [Authorize]
        public IActionResult NewTransaction(Transaction model) {
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
