using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvrakOnizlemeController(IGidenEvrakService giden,IGelenEvrakService gelen) : ControllerBase
    {

        [HttpGet("EkGoruntule/{evrakTipi}/{ekId}")]
        public async Task<IActionResult> EkGoruntule(string evrakTipi, int ekId)
        {
        
            byte[] dosyaVerisi = null;

            if (evrakTipi.ToLower() == "gelen")
            {
                var ek = await gelen.GelenEvrakEkOnizlemeAsync(ekId);
                dosyaVerisi = ek?.DosyaVerisi;
            }
            else if (evrakTipi.ToLower() == "giden")
            {
                var ek = await giden.GidenEvrakEkOnizlemeAsync(ekId);
                dosyaVerisi = ek?.DosyaVerisi;
            }

            if (dosyaVerisi == null) 
                throw new NotFoundException("Dosya bulunamadı.");

            return File(dosyaVerisi, "application/pdf");
        }


    }
}
