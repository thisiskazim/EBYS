
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvrakController(IEvrakService evrakServive, IKonuKoduService konuKoduService) : ControllerBase
    {

        [HttpPost("EvrakOlustur")]
        public async Task<IActionResult> EvrakOlustur(GidenEvrakCreateDTO evrakCreateDTO)
        {

            try
            {
                await evrakServive.AddAsync(evrakCreateDTO);
                return Ok("Evrak başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost("ParafımıBekleyenler")]
        public async Task<IActionResult> ParafimiBekleyenler(GidenEvrakCreateDTO evrakCreateDTO)
        {

            try
            {
                await evrakServive.AddAsync(evrakCreateDTO);
                return Ok("Evrak başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }


        [HttpGet("KonuKoduGet")]
        public async Task<IActionResult> KonuKoduGet()
        {
            try
            {
                var konuKodlari = await konuKoduService.KonuKoduList();
                return Ok(konuKodlari);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
