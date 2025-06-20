using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DWebProjetoFinal.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var hoje = DateTime.Now;
            var primeiroDiaDoMes = new DateTime(hoje.Year, hoje.Month, 1);

            var transacoes = _context.Transacoes
                .Where(t => t.UserId == userId && t.Data >= primeiroDiaDoMes && t.Data <= hoje)
                .ToList();

            var receitaMensal = transacoes
                .Where(t => t.TipoTransacao == "Receita")
                .Sum(t => t.Valor);

            var despesaMensal = transacoes
                .Where(t => t.TipoTransacao == "Despesa")
                .Sum(t => t.Valor);

            var saldo = receitaMensal - despesaMensal;

            var categoriasDespesa = transacoes
                .Where(t => t.TipoTransacao == "Despesa")
                .GroupBy(t => t.Categoria)
                .Select(g => new { Categoria = g.Key, Total = g.Sum(t => t.Valor) })
                .ToList();

            var transacoesPorDia = transacoes
                .GroupBy(t => t.Data.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Dia = g.Key.ToString("dd/MM"),
                    ValorDiario = g.Sum(t => t.TipoTransacao == "Receita" ? t.Valor : -t.Valor)
                })
                .ToList();

            var metas = _context.Metas
                .Where(m => m.UserId == userId)
                .OrderBy(m => m.DataLimite)
                .ToList();

            List<object> saldoAcumulado = new();
            decimal saldoAtual = 0;

            foreach (var item in transacoesPorDia)
            {
                saldoAtual += item.ValorDiario;
                saldoAcumulado.Add(new { Dia = item.Dia, Saldo = saldoAtual });
            }

            ViewBag.Receita = receitaMensal;
            ViewBag.Despesa = despesaMensal;
            ViewBag.Saldo = saldo;
            ViewBag.Categorias = categoriasDespesa;
            ViewBag.Diario = saldoAcumulado;
            ViewBag.Metas = metas;

            return View();
        }
    }
}