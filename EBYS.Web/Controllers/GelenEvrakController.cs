using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class GelenEvrakController : Controller
    {
        public IActionResult GelenEvrak(int? id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult GelenEvrakListe()
        {
            return View();
        }
    }
}