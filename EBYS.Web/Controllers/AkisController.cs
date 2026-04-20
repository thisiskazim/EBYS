using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class AkisController : Controller
    {
        public IActionResult ImzaBekleyenListele()
        {
            return View();
        }
        public IActionResult ParafBekleyenListele()
        {
            return View();
        }
        public IActionResult ImzayaGonderdiklerimListe()
        {
            return View();
        }
    }
}
