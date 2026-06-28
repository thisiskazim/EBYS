using EBYS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EBYS.Web.Controllers
{
    public class GidenEvrakController : Controller
    {

        public IActionResult GidenEvrakOlustur(int? EvrakId)
        {
            if (EvrakId.HasValue)
            {
                ViewBag.EvrakId = EvrakId.Value;
            }
            return View();
        }
        public IActionResult GidenEvrakListe()
        {
         
            return View();
        }


    }
}
