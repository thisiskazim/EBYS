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
                return Ok(sonuc); 

            return BadRequest(sonuc); 
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

        [HttpPost("Reddet/{id}")]
        public async Task<IActionResult> Reddet(int id, [FromQuery] string not)
        {
            var sonuc = await akisService.ReddetAsync(id, not);

            if (sonuc.BasariliMi)
                return Ok(sonuc);

            return BadRequest(sonuc);
        }

        [HttpPost("IadeEt/{id}")]
        public async Task<IActionResult> IadeEt(int id, [FromQuery] string not)
        {
            var sonuc = await akisService.IadeEtAsync(id, not);

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
