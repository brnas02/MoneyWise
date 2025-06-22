using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models;
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
        public IActionResult Transacoes(int? mes, int? ano)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var dataAgora = DateTime.Now;
            int mesSelecionado = mes ?? dataAgora.Month;
            int anoSelecionado = ano ?? dataAgora.Year;

            var historico = _context.Transacoes
                .Where(t => t.UserId == userId && t.Data.Month == mesSelecionado && t.Data.Year == anoSelecionado)
                .OrderByDescending(t => t.Data)
                .ToList();

            var viewModel = new TransacaoViewModel
            {
                NovaTransacao = new Transacao { Data = DateTime.Now },
                Historico = historico,
                Mes = mesSelecionado,
                Ano = anoSelecionado
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(TransacaoViewModel model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid)
            {
                model.Historico = _context.Transacoes
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Data)
                    .ToList();

                return View("Transacoes", model);
            }

            model.NovaTransacao.UserId = userId;
            _context.Transacoes.Add(model.NovaTransacao);
            _context.SaveChanges();

            return RedirectToAction("Transacoes");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var transacao = _context.Transacoes.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
                _context.SaveChanges();
            }

            return RedirectToAction("Transacoes");
        }
    }
}
