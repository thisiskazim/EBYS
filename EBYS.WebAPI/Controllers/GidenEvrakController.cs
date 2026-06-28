
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Services;
using EBYS.Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GidenEvrakController(IGidenEvrakService evrakServive, IGidenEvrakAkisService akisService,IKonuKoduService konuKoduService) : ControllerBase
    {

        [HttpPost("EvrakOlustur")]
        public async Task<IActionResult> EvrakOlustur([FromForm] GidenEvrakCreateDTO evrakCreateDTO)
        {
                await evrakServive.AddAsync(evrakCreateDTO);
                return Ok("Evrak başarıyla kaydedildi");
        }


        [HttpPost("EvrakGuncelle")]
        public async Task<IActionResult> EvrakGuncelle([FromForm] GidenEvrakUpdateDTO evrakCreateDTO)
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

        [HttpDelete("EvrakSil/{id}")]
        public async Task<IActionResult> EvrakSil(int id)
        {
            try
            {
                await evrakServive.DeleteAsync(id);
                return Ok("Evrak silindi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("EvrakListele")]
        public async Task<IActionResult> EvrakListele([FromForm] Enums.GidenEvrakFiltreTipi? filtreTipi)
        {
            try
            {
                var evraklar = await evrakServive.GidenEvraklariFiltreliListeleAsync(filtreTipi);
                return Ok(evraklar);
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

        [HttpGet("evrak-hareketleri/{id}")]
        public async Task<IActionResult> EvrakHareketleri(int id)
        {

            try
            {
                var gelenVeri = await akisService.EvrakHareketleriGetirAsync(id);

                if (gelenVeri == null)
                {
                    return NotFound("Evrak Hareketleri Bulunamadı");
                }

                return Ok(gelenVeri);
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
                    return NotFound("Böyle bir evrak bulunamadı.");
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
