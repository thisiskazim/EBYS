using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using Microsoft.AspNetCore.Mvc;


namespace EBYS.WebAPI.Controllers.Muhatap
{
    [Route("api/[controller]")]
    [ApiController]
    public class TuzelKisiMuhatapController(IMuhatapTuzelKisiService tuzelKisiService, IMapper mapper) : ControllerBase
    {

        [HttpGet("Listele")]
        public async Task<IActionResult> TuzelKisiListele()
        {
            try
            {
                var getVeri = await tuzelKisiService.GetAllAsync();

                if (getVeri == null || !getVeri.Any())
                {
                    return NotFound("TuzelKisi bulunamadı.");
                }

                return Ok(getVeri);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }

        [HttpPost("Ekle")]
        public async Task<IActionResult> TuzelKisiEkle(TuzelKisiMuhatapCreateDTO dto)
        {
            try
            {
                await tuzelKisiService.AddAsync(dto);
                return Ok("TuzelKisi başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost("Guncelle")]
        public async Task<IActionResult> TuzelKisiGuncelle(TuzelKisiMuhatapUpdateDTO dto)
        {
            try
            {
                await tuzelKisiService.UpdateAsync(dto);
                return Ok("TuzelKisi başarıyla güncellendi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Sil/{id}")]
        public async Task<IActionResult> TuzelKisiSil(int id)
        {
            try
            {
                await tuzelKisiService.DeleteAsync(id);
                return Ok("Rota silindi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Getir/{id}")]
        public async Task<IActionResult> TuzelKisiGetir(int id)
        {
            try
            {
                var gelenVeri = await tuzelKisiService.GetByIdAsync(id);
                if (gelenVeri == null)
                {
                    return NotFound("Böyle bir TuzelKisi bulunamadı.");
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
