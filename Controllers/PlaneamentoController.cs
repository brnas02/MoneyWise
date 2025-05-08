using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    [Authorize]
    public class PlaneamentoController : Controller
    {
        private readonly AppDbContext _context;

        public PlaneamentoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Orcamento()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orcamentos = _context.Orcamentos
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.MesReferencia)
                .ToList();

            return View(orcamentos);
        }

        [HttpPost]
        public IActionResult Orcamento(Orcamento model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                _context.Orcamentos.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Orcamento");
            }

            return View(new List<Orcamento>()); // fallback
        }

        [HttpPost]
        [Authorize]
        public IActionResult ApagarOrcamento(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var orcamento = _context.Orcamentos.FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (orcamento != null)
            {
                _context.Orcamentos.Remove(orcamento);
                _context.SaveChanges();
            }

            return RedirectToAction("Orcamento");
        }
    }
}
