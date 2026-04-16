using EBYS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParafBekleyenEvrakController(IAkisService akisService) : ControllerBase
    {
        [HttpGet("Listele")]
        public async Task<IActionResult> Listele()
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
        public async Task<IActionResult> Onayla(int id)
        {
            var sonuc = await akisService.OnaylaAsync(id);

            if (sonuc.BasariliMi)
                return Ok(sonuc); // 200 döner

            return BadRequest(sonuc); // 400 döner ve içindeki mesajı verir
        }

    }
}
