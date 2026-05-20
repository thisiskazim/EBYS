using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class GelenEvrakController : Controller
    {
        public IActionResult GelenEvrak(int? EvrakId)
        {
            ViewBag.EvrakId = EvrakId;
            return View();
        }

        public IActionResult GelenEvrakListe()
        {
            return View();
        }
    }
}