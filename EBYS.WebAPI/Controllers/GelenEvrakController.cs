using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GelenEvrakController(IGelenEvrakService evrakServive, IKonuKoduService konuKoduService) : ControllerBase
    {
        [HttpPost("EvrakOlustur")]
        public async Task<IActionResult> EvrakOlustur([FromForm] GelenEvrakCreateDTO evrakCreateDTO)
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




    }
}
