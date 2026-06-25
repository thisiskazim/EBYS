using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class GidenEvrakAkisController : Controller
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

        public IActionResult TumGidenEvraklarListele()
        {
            return View();
        }
    }
}
