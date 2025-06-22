using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DWebProjetoFinal.Controllers
{
    // Apenas utilizadores com roles "Empresarial" ou "Admin" podem aceder a este controller
    [Authorize(Roles = "Empresarial,Admin")]
    public class RelatorioController : Controller
    {
        // Ação GET que retorna a view do relatório mensal sem dados filtrados (ex. tela inicial com formulário)
        [HttpGet]
        public IActionResult Mensal()
        {
            return View();
        }

        // Ação POST que recebe os parâmetros mês e ano para gerar o relatório mensal
        [HttpPost]
        public IActionResult Mensal(int mes, int ano)
        {
            // No estado atual, o método apenas retorna a mesma view.
            // Aqui pode-se implementar a lógica para carregar e exibir os dados filtrados por mês/ano.
            return View();
        }

        // Ação GET que retorna a view do relatório anual
        [HttpGet]
        public IActionResult Anual()
        {
            return View();
        }
    }
}