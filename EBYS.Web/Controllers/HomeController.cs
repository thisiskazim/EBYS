using EBYS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EBYS.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index(int? id)
        {
            if (id.HasValue)
            {
                ViewBag.EvrakId = id.Value;
            }
            return View();
        }

      }
}
