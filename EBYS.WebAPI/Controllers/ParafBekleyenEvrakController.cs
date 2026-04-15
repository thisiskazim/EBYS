using EBYS.Application.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParafBekleyenEvrakController(IEvrakService evrakService) : ControllerBase
    {
        [HttpGet("Listele")]
        public async Task<IActionResult> Listele()
        {
            try
            {
                var data = await evrakService.ImzaBekleyenListe();

                return Ok(data);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }

        }
    }
}
