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

        // ---- Métodos para Metas ----
        public IActionResult Metas()
        {
            var userId = GetUserId();
            var metas = _context.Metas.Where(m => m.UserId == userId).ToList();
            return View(metas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarMeta(Meta model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = GetUserId();
                _context.Metas.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Metas");
            }
            return View("Metas", _context.Metas.Where(m => m.UserId == GetUserId()).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarMeta(int id)
        {
            var userId = GetUserId();
            var meta = _context.Metas.FirstOrDefault(m => m.Id == id && m.UserId == userId);

            if (meta == null) return NotFound();

            _context.Metas.Remove(meta);
            _context.SaveChanges();
            return RedirectToAction("Metas");
        }

        // ---- Métodos para Orçamento ----
        public IActionResult Orcamento()
        {
            var userId = GetUserId();
            var orcamentos = _context.Orcamentos
                .Where(o => o.UserId == userId && o.Mes == DateTime.Now.Month && o.Ano == DateTime.Now.Year)
                .ToList();
            return View(orcamentos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarOrcamento(Orcamento model)
        {
            if (ModelState.IsValid)
            {
                model.UserId = GetUserId();
                model.Mes = DateTime.Now.Month;
                model.Ano = DateTime.Now.Year;
                _context.Orcamentos.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Orcamento");
            }
            return View("Orcamento", _context.Orcamentos.Where(o => o.UserId == GetUserId()).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarOrcamento(int id)
        {
            var userId = GetUserId();
            var orcamento = _context.Orcamentos.FirstOrDefault(o => o.Id == id && o.UserId == userId);

            if (orcamento == null) return NotFound();

            _context.Orcamentos.Remove(orcamento);
            _context.SaveChanges();
            return RedirectToAction("Orcamento");
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}