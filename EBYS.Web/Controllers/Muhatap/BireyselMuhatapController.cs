using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers.Muhatap
{
    public class BireyselMuhatapController : Controller
    {
        public IActionResult Ekle(int? id)
        {
            ViewBag.Id = id ?? 0;
            return View();
        }
        public IActionResult Listele(int id)
        {
            return View(id);
        }
    }
}
