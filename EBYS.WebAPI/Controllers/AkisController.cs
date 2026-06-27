using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using Microsoft.AspNetCore.Mvc;


namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AkisController(IGidenEvrakAkisService akisService) : ControllerBase
    {
        [HttpGet("imza-bekleyen-listele")]
        public async Task<IActionResult> ImzaBekleyenEvrakListele()
        {
             var data = await akisService.ImzaBekleyenleriGetirAsync();
             return Ok(data);
        }


        [HttpGet("paraf-bekleyen-listele")]
        public async Task<IActionResult> ParafBekleyenEvrakListele()
        {
             var data = await akisService.ParafBekleyenleriGetirAsync();
             return Ok(data);
        }


        [HttpPost("Onayla/{id}")]
        public async Task<IActionResult> EvrakOnayla(int id)
        {
            var sonuc = await akisService.OnaylaAsync(id);

            if (sonuc.BasariliMi)
                return Ok(sonuc); 

            return BadRequest(sonuc); 
        }

        [HttpGet("imzaya-gonderdiklerim")]
        public async Task<IActionResult> ImzayaGonderdiklerim()
        {
            var data = await akisService.ImzayaGonderdigimAsync();
            return Ok(data);
        }


        [HttpPost("GeriCek/{id}")]
        public async Task<IActionResult> GeriCek(int id)
        {
            var sonuc = await akisService.GeriCekAsync(id);

            if (sonuc.BasariliMi)
                return Ok(sonuc);

            return BadRequest(sonuc);
        }

        [HttpGet("evrak-hareketleri/{id}")]
        public async Task<IActionResult> EvrakHareketleri(int id)
        {
             var data = await akisService.EvrakHareketleriGetirAsync(id);
             return Ok(data);
        }
    }
}
