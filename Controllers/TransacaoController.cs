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
            var historico = _context.UserTransacao
                .Where(ut => ut.UserAccountId == userId &&
                             ut.Transacao.Data.Month == mesSelecionado &&
                             ut.Transacao.Data.Year == anoSelecionado)
                .Select(ut => ut.Transacao)
                .OrderByDescending(t => t.Data)
                .ToList();

            return View(historico);
        }

        // Cria uma nova transação (Receita ou Despesa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Transacao model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (!ModelState.IsValid)
            {
                return View("Transacoes", model);
            }

            // 1. Adiciona a transação
            _context.Transacoes.Add(model);
            _context.SaveChanges();

            // 2. Cria a relação na tabela de junção
            var userTransacao = new UserTransacao
            {
                UserAccountId = userId,
                TransacaoId = model.Id
            };

            _context.Add(userTransacao);
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

            var transacao = _context.UserTransacao
                .Where(ut => ut.UserAccountId == userId && ut.TransacaoId == id)
                .Select(ut => ut.Transacao)
                .FirstOrDefault();

            if (transacao != null)
            {
                _context.Transacoes.Remove(transacao);
                _context.SaveChanges();
            }

            return RedirectToAction("Transacoes");
        }
    }
}