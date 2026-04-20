
using EBYS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;


namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AkisController(IAkisService akisService) : ControllerBase
    {
        [HttpGet("imza-bekleyen-listele")]
        public async Task<IActionResult> ImzaBekleyenEvrakListele()
        {
            try
            {
                var data = await akisService.ImzaBekleyenleriGetirAsync();

                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }


        [HttpGet("paraf-bekleyen-listele")]
        public async Task<IActionResult> ParafBekleyenEvrakListele()
        {
            try
            {
                var data = await akisService.ParafBekleyenleriGetirAsync();

                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }



        [HttpPost("Onayla/{id}")]
        public async Task<IActionResult> EvrakOnayla(int id)
        {
            var sonuc = await akisService.OnaylaAsync(id);

            if (sonuc.BasariliMi)
                return Ok(sonuc); // 200 döner

            return BadRequest(sonuc); // 400 döner ve içindeki mesajı verir
        }

        [HttpGet("imzaya-gonderdiklerim")]
        public async Task<IActionResult> ImzayaGonderdiklerim()
        {
            try
            {
                var data = await akisService.ImzayaGonderdigimAsync();

                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

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
            try
            {
                var data = await akisService.EvrakHareketleriGetirAsync(id);

                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }



    }
}
