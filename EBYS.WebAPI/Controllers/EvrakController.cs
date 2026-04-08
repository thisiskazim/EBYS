using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService.IEvrakService;
using EBYS.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvrakController(IEvrakService evrakServive) : ControllerBase
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
    }
}
