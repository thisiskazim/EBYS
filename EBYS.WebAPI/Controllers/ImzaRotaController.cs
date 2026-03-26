using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImzaRotaController(IImzaRotaService imzaRotaService) : Controller
    {

        [HttpPost("ImzaRotaEkle")]
        public async Task<IActionResult> ImzaRotaEkle(ImzaRotaCreateDTO ımzaRotaCreateDTO)
        { 
   
            try
            {
                await imzaRotaService.AddImzaRotaAsync(ımzaRotaCreateDTO);
                return Ok("Imza Rota başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }
    }
}
