using Microsoft.AspNetCore.Mvc;


namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImzaBekleyenEvrakController : ControllerBase
    {
        // GET: api/<ImzaBekleyenEvrakController>
        [HttpGet("Listele")]
        public async Task<IActionResult> Listele()
        {
            return 0;
        }

      
    }
}
