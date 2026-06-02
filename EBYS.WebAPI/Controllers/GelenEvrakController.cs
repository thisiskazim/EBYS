using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GelenEvrakController(IGelenEvrakService evrakServive, IKonuKoduService konuKoduService) : ControllerBase
    {
        [HttpPost("EvrakOlustur")]
        public async Task<IActionResult> EvrakOlustur([FromForm] GelenEvrakCreateDTO evrakCreateDTO)
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


        [HttpPost("EvrakListele")]
        public async Task<IActionResult> EvrakListele([FromForm] Enums.GelenEvrakDurumu? durum)
        {
            try
            {
                var evraklar = await evrakServive.GelenEvraklariFiltreliListeleAsync(durum);
                return Ok(evraklar);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

         [HttpPost("EvrakGuncelle")]
        public async Task<IActionResult> EvrakGuncelle([FromForm] GelenEvrakUpdateDTO evrakUpdateDTO)
        {

            try
            {
                await evrakServive.UpdateAsync(evrakUpdateDTO);
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


        [HttpGet("evrak-sevk-hareketleri/{id}")]
        public async Task<IActionResult> EvrakSevkHareketleri(int id)
        {

            try
            {
                var gelenVeri = await evrakServive.GelenEvrakHareketleri(id);

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


        [HttpPost("SahsimaTeslimAl/{id}")]
        public async Task<IActionResult> SahsimaTeslimAl(int id)
        {

            try
            {
                var teslimAl = await evrakServive.SahsimaTeslimAl(id);
                return Ok(teslimAl);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet("EvrakPdfGoruntule/{ekId}")]
        public async Task<IActionResult> EvrakPdfGoruntule(int ekId)
        {
            try
            {
                var ek = await evrakServive.GelenEvrakEkOnizlemeAsync(ekId);

                if (ek == null || ek.DosyaVerisi == null) return NotFound();

                return File(ek.DosyaVerisi, "application/pdf");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }




    }
}
