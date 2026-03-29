using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Application.Services;
using EBYS.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImzaRotaController(IImzaRotaService imzaRotaService, IImzaRotaRepository imzaRotaRepository)
        : ControllerBase
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


        [HttpPost("ImzaRotaGuncelle")]
        public async Task<IActionResult> ImzaRotaGuncelle(ImzaRotaUpdateDTO ımzaRotaCreateDTO)
        {

            try
            {
                await imzaRotaService.UpdateImzaRotaAsync(ımzaRotaCreateDTO);
                return Ok("Imza Rota başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }


        [HttpGet("ImzaRotaGetir/{id}")]
        public async Task<IActionResult> ImzaRotaGetById(int id)
        {

            try
            {
                var gelenVeri = await imzaRotaService.ImzaRotaGetByIdAsycn(id);

                if (gelenVeri == null)
                {
                    return NotFound("Böyle bir rota bulunamadı.");
                }

                return Ok(gelenVeri);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }


        [HttpGet("ImzaRotaListesi")]
        public async Task<IActionResult> ImzaRotaListAsync()
        {
            try
            {
                var getList = await imzaRotaService.ImzaRotaListAsync();
                if (getList == null)
                {
                    return NotFound("Imza Rota Listesi Boş");
                }

                return Ok(getList);


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
        
        [HttpDelete("ImzaRotaSil/{id}")]
        public async Task<IActionResult> ImzaRotaSil(int id)
        {
            try
            {
                await imzaRotaService.DeleteImzaRotaAsync(id);
                return Ok("Rota silindi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
