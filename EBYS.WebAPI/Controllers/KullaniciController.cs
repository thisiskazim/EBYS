using EBYS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController(IKullaniciService kullaniciService) : Controller
    {

       
        [HttpGet("KullanicilariListele")]
        public async Task<IActionResult> GetKullaniciAll()
        { 
            var kullanicilar = await kullaniciService.GetKullaniciAll();
            
            return Ok(kullanicilar);
        }
    }
}
