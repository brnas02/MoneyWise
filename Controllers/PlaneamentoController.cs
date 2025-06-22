using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    // Apenas utilizadores autenticados podem aceder às ações deste controller
    [Authorize]
    public class PlaneamentoController : Controller
    {
        private readonly AppDbContext _context;

        // Injeção de dependência do contexto da base de dados
        public PlaneamentoController(AppDbContext context)
        {
            _context = context;
        }

        // ----------------- MÉTODOS PARA METAS -----------------

        // Exibe metas do utilizador para o mês e ano selecionado (ou atual, se não fornecido)
        public IActionResult Metas(int? mes, int? ano)
        {
            var userId = GetUserId();
            var mesSelecionado = mes ?? DateTime.Now.Month;
            var anoSelecionado = ano ?? DateTime.Now.Year;

            // Busca metas do utilizador filtrando por mês e ano
            var metas = _context.Metas
                .Where(m => m.UserId == userId &&
                            m.DataLimite.Month == mesSelecionado &&
                            m.DataLimite.Year == anoSelecionado)
                .ToList();

            ViewBag.Mes = mesSelecionado;
            ViewBag.Ano = anoSelecionado;

            return View(metas);
        }

        // Cria nova meta para o utilizador atual
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

            // Caso o modelo seja inválido, recarrega a lista de metas para exibir novamente a view
            return View("Metas", _context.Metas.Where(m => m.UserId == GetUserId()).ToList());
        }

        // Elimina meta com base no ID e pertencente ao utilizador autenticado
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

        // ----------------- MÉTODOS PARA ORÇAMENTOS -----------------

        // Exibe orçamentos do utilizador para o mês e ano selecionado (ou atual, se não fornecido)
        public IActionResult Orcamento(int? mes, int? ano)
        {
            var userId = GetUserId();
            var mesSelecionado = mes ?? DateTime.Now.Month;
            var anoSelecionado = ano ?? DateTime.Now.Year;

            var orcamentos = _context.Orcamentos
                .Where(o => o.UserId == userId &&
                            o.Mes == mesSelecionado &&
                            o.Ano == anoSelecionado)
                .ToList();

            ViewBag.Mes = mesSelecionado;
            ViewBag.Ano = anoSelecionado;

            return View(orcamentos);
        }

        // Cria novo orçamento para o mês e ano atual
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

            // Recarrega a lista de orçamentos se o modelo não for válido
            return View("Orcamento", _context.Orcamentos.Where(o => o.UserId == GetUserId()).ToList());
        }

        // Elimina um orçamento pelo ID, desde que pertença ao utilizador atual
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

        // Método auxiliar para obter o ID do utilizador autenticado a partir das claims
        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}