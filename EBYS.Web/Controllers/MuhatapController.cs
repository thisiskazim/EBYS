using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class MuhatapController : Controller
    {
        public IActionResult EkleKurum()
        {
            return View();
        }
        public IActionResult EkleBireyselVatandas()
        {
            return View();
        }

        public IActionResult EkleTuzelKisi()
        {
            return View();
        }


    }
}
