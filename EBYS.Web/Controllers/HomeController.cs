using EBYS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EBYS.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(int? EvrakId)
        {
            if (EvrakId.HasValue)
            {
                ViewBag.EvrakId = EvrakId.Value;
            }
            return View();
        }

      }
}
