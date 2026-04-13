using EBYS.Application.Interfaces.IService;
using EBYS.Application.Services.EvrakService;
using Microsoft.AspNetCore.Mvc;


namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImzaBekleyenEvrakController(IEvrakService evrakService) : ControllerBase
    {
        // GET: api/<ImzaBekleyenEvrakController>
        [HttpGet("Listele")]
        public async Task<IActionResult> Listele()
        {
            var data = await evrakService.ImzaBekleyenListe();

            return Ok(data);
        }


    }
}
