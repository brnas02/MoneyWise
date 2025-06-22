using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DWebProjetoFinal.Controllers
{
    [Authorize(Roles = "Empresarial,Admin")]
    public class RelatorioController : Controller
    {
        [HttpGet]
        public IActionResult Mensal()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Mensal(int mes, int ano)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Anual()
        {
            return View();
        }
    }
}
