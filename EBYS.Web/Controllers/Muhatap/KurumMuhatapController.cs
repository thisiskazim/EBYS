using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers.Muhatap
{
    public class KurumMuhatapController : Controller
    {
        public IActionResult Ekle(int? id)
        {
            ViewBag.KurumId = id ?? 0;
            return View();
        }
        public IActionResult Listele(int id) 
        {
            return View(id);
        }


    }
}
