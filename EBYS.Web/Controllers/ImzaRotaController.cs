using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class ImzaRotaController : Controller
    {
        public IActionResult ImzaRotaAdd(int? id)
        {
            ViewBag.RotaId = id ?? 0;
            return View();
        }

        public IActionResult ImzaRotaListe()
        {
            return View();
        }

    }
}
