using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers.Muhatap
{

    [Route("api/[controller]")]
    [ApiController]
    public class MuhatapController(IMuhatapRepository muhatapRepository,IMapper mapper) : ControllerBase//bunda şimdilik liste alacağım için servis oluşturmadım
    {
        [HttpGet("TumMuhataplar")]
        public async Task<IActionResult> Get()
        {
           
            var liste = await muhatapRepository.GetAllAsync();

          
            var map = mapper.Map<List<MuhatapBaseDTO>>(liste);

            return Ok(map);
        }
    }
}
