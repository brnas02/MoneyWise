using DWebProjetoFinal.Entities;
using DWebProjetoFinal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    // Apenas utilizadores autenticados podem aceder a este controller
    [Authorize]
    public class TransacaoController : Controller
    {
        private readonly AppDbContext _context;

        // Injeção do contexto da base de dados
        public TransacaoController(AppDbContext context)
        {
            _context = context;
        }

        // Exibe o histórico de transações para o mês/ano selecionado
        [HttpGet]
        public IActionResult Transacoes(int? mes, int? ano)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var dataAgora = DateTime.Now;
            int mesSelecionado = mes ?? dataAgora.Month;
            int anoSelecionado = ano ?? dataAgora.Year;

            // Busca as transações do utilizador filtradas por mês e ano
            var historico = _context.Transacoes
                .Where(t => t.UserId == userId && t.Data.Month == mesSelecionado && t.Data.Year == anoSelecionado)
                .OrderByDescending(t => t.Data)
                .ToList();

            // Cria o view model contendo uma nova transação e o histórico
            var viewModel = new TransacaoViewModel
            {
                NovaTransacao = new Transacao { Data = DateTime.Now }, // inicializa a data como agora
                Historico = historico,
                Mes = mesSelecionado,
                Ano = anoSelecionado
            };

            return View(viewModel);
        }

        // Cria uma nova transação (Receita ou Despesa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(TransacaoViewModel model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Se o modelo for inválido, recarrega o histórico e retorna a view com os dados preenchidos
            if (!ModelState.IsValid)
            {
                model.Historico = _context.Transacoes
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.Data)
                    .ToList();

                return View("Transacoes", model);
            }

            // Associa o ID do utilizador à nova transação
            model.NovaTransacao.UserId = userId;
            _context.Transacoes.Add(model.NovaTransacao);
            _context.SaveChanges();

            // Redireciona para a listagem de transações após criar
            return RedirectToAction("Transacoes");
        }

        // Elimina uma transação com base no ID (apenas se pertencer ao utilizador)
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