using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers.Muhatap
{
    [Route("api/[controller]")]
    [ApiController]
    public class BireyselMuhatapController(IMuhatapBireyselService muhatapBireyselService, IMapper mapper) : ControllerBase
    {
        [HttpGet("Listele")]
        public async Task<IActionResult> VatandasListele()
        {
            try
            {
                var getVeri = await muhatapBireyselService.GetAllAsync();

                if (getVeri == null || !getVeri.Any())
                {
                    return NotFound("Vatandaş bulunamadı.");
                }

                return Ok(getVeri);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Ekle")]
        public async Task<IActionResult> VatandasEkle(BireyselMuhatapCreateDTO dto)
        {
            try
            {
                await muhatapBireyselService.AddAsync(dto);
                return Ok("Vatandaş başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Guncelle")]
        public async Task<IActionResult> VatandasGuncelle(BireyselMuhatapUpdateDTO dto)
        {
            try
            {
                await muhatapBireyselService.UpdateAsync(dto);
                return Ok("Vatandaş başarıyla güncellendi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Sil/{id}")]
        public async Task<IActionResult> VatandasSil(int id)
        {
            try
            {
                await muhatapBireyselService.DeleteAsync(id);
                return Ok("Rota silindi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Getir/{id}")]
        public async Task<IActionResult> VatandasGetir(int id)
        {
            try
            {
                var gelenVeri = await muhatapBireyselService.GetByIdAsync(id);
                if (gelenVeri == null)
                {
                    return NotFound("Böyle bir vatandaş bulunamadı.");
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
