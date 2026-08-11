using EBYS.Application.DTOs.ResmiYaziDTO;
using EBYS.Application.Interfaces.IService.IResmiYaziService;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResmiYaziController(IResmiYaziGeneratorService resmiYaziService) : ControllerBase
    {
        [HttpPost("ResmiMetinOlustur")]
        [HttpPost("Generate")]
        public async Task<IActionResult> ResmiMetinOlustur([FromBody] ResmiYaziGenerateRequest request)
        {
            var result = await resmiYaziService.ResmiMetinOlusturAsync(request);
            return Ok(result);
        }
    }
}
