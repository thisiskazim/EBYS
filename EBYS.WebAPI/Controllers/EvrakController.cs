
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Services;
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


        [HttpPost("EvrakGuncelle")]
        public async Task<IActionResult> EvrakGuncelle(GidenEvrakUpdateDTO evrakCreateDTO)
        {

            try
            {
                await evrakServive.UpdateAsync(evrakCreateDTO);
                return Ok("Evrak başarıyla güncellendi");
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



        [HttpGet("EvrakGetir/{id}")]
        public async Task<IActionResult> EvrakGetirGetById(int id)
        {

            try
            {
                var gelenVeri = await evrakServive.GetByIdAsync(id);

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
    }
}
