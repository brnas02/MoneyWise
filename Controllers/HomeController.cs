using DWebProjetoFinal.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DWebProjetoFinal.Controllers
{
    // Aplica a autorização a todas as ações, exceto as marcadas com [AllowAnonymous]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        // Injeção de dependência para o logger e o contexto da base de dados
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Página pública de apresentação (landing page)
        [AllowAnonymous]
        public IActionResult Apresentacao()
        {
            return View();
        }

        // Dashboard principal do utilizador autenticado
        public IActionResult Dashboard(int? mes, int? ano)
        {
            // Obtém o ID do utilizador logado através das claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // Define mês e ano selecionados, ou usa o mês/ano atual
            var agora = DateTime.Now;
            int mesSelecionado = mes ?? agora.Month;
            int anoSelecionado = ano ?? agora.Year;

            // Define o intervalo de datas para o mês selecionado
            var primeiroDiaDoMes = new DateTime(anoSelecionado, mesSelecionado, 1);
            var ultimoDiaDoMes = new DateTime(anoSelecionado, mesSelecionado, DateTime.DaysInMonth(anoSelecionado, mesSelecionado));

            // Busca todas as transações do utilizador dentro do intervalo do mês
            var transacoes = _context.Transacoes
                .Where(t => t.UserId == userId && t.Data >= primeiroDiaDoMes && t.Data <= ultimoDiaDoMes)
                .ToList();

            // Soma de receitas no mês
            var receitaMensal = transacoes
                .Where(t => t.TipoTransacao == "Receita")
                .Sum(t => t.Valor);

            // Soma de despesas no mês
            var despesaMensal = transacoes
                .Where(t => t.TipoTransacao == "Despesa")
                .Sum(t => t.Valor);

            // Saldo calculado (receitas - despesas)
            var saldo = receitaMensal - despesaMensal;

            // Agrupamento das despesas por categoria
            var categoriasDespesa = transacoes
                .Where(t => t.TipoTransacao == "Despesa")
                .GroupBy(t => t.Categoria)
                .Select(g => new { Categoria = g.Key, Total = g.Sum(t => t.Valor) })
                .ToList();

            // Agrupamento de transações por dia, convertendo receitas em +valor e despesas em -valor
            var transacoesPorDia = transacoes
                .GroupBy(t => t.Data.Date)
                .OrderBy(g => g.Key)
                .Select(g => new {
                    Dia = g.Key.ToString("dd/MM"),
                    ValorDiario = g.Sum(t => t.TipoTransacao == "Receita" ? t.Valor : -t.Valor)
                })
                .ToList();

            // Busca das metas do utilizador para o mês/ano selecionado
            var metas = _context.Metas
                .Where(m => m.UserId == userId &&
                            m.DataLimite.Month == mesSelecionado &&
                            m.DataLimite.Year == anoSelecionado)
                .OrderBy(m => m.DataLimite)
                .ToList();

            // Busca dos orçamentos do utilizador para o mês/ano selecionado
            var orcamentos = _context.Orcamentos
                .Where(o => o.UserId == userId && o.Mes == mesSelecionado && o.Ano == anoSelecionado)
                .ToList();

            // Cálculo do uso percentual dos orçamentos
            var limiteTotal = orcamentos.Sum(o => o.LimiteMensal);
            var gastoTotal = orcamentos.Sum(o => o.GastoAtual);
            var usoPercentagem = limiteTotal > 0 ? (gastoTotal / limiteTotal * 100) : 0;

            // Criação de lista com saldo acumulado ao longo dos dias
            List<object> saldoAcumulado = new();
            decimal saldoAtual = 0;

            foreach (var item in transacoesPorDia)
            {
                saldoAtual += item.ValorDiario;
                saldoAcumulado.Add(new { Dia = item.Dia, Saldo = saldoAtual });
            }

            // Envio dos dados para a view através do ViewBag
            ViewBag.Receita = receitaMensal;
            ViewBag.Despesa = despesaMensal;
            ViewBag.Saldo = saldo;
            ViewBag.Categorias = categoriasDespesa;
            ViewBag.Diario = saldoAcumulado;
            ViewBag.Metas = metas;
            ViewBag.OrcamentoLimite = limiteTotal;
            ViewBag.OrcamentoGasto = gastoTotal;
            ViewBag.OrcamentoPercent = usoPercentagem;
            ViewBag.Mes = mesSelecionado;
            ViewBag.Ano = anoSelecionado;

            return View();
        }
    }
}