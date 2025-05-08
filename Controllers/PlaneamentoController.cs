using Microsoft.AspNetCore.Mvc;

namespace DWebProjetoFinal.Controllers {
    public class PlaneamentoController : Controller {
        public IActionResult Metas() {
            return View();
        }

        public IActionResult Orcamento() {
            return View();
        }
    }
}
