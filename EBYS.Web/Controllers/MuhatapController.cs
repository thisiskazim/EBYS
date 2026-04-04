using Microsoft.AspNetCore.Mvc;

namespace EBYS.Web.Controllers
{
    public class MuhatapController : Controller
    {
        public IActionResult EkleKurum(int? id)
        {
            ViewBag.KurumId = id ?? 0;
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
        public IActionResult KurumListele(int id) 
        {
            return View(id);
        }


    }
}
