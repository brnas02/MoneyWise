using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    [Authorize]
    public class TransacaoController : Controller
    {
        private readonly AppDbContext _context;

        public TransacaoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult NovaTransacao()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult NovaTransacao(Transacao model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            model.UserId = int.Parse(userIdClaim);
            _context.Transactions.Add(model);
            _context.SaveChanges();

            return RedirectToAction("Historico");
        }

        [HttpGet]
        public IActionResult Historico()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
                return Unauthorized();

            int userId = int.Parse(userIdClaim);
            var transacoes = _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Data)
                .ToList();

            return View(transacoes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var transacao = _context.Transactions.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (transacao == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transacao);
            _context.SaveChanges();

            return RedirectToAction("Historico");
        }

    }
}
