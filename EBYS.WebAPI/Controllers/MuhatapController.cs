using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Services;
using EBYS.Domain.Entities;
using EBYS.Persistence;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI; // Kendo paketini kurmuş olmalısın
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuhatapController(IMuhatapKurumService muhatapKurumService, IMapper mapper) : ControllerBase
    {

        [HttpGet("KurumListele")]
        public async Task<IActionResult> KurumListele()
        {
            try
            {
                var getVeri = await muhatapKurumService.GetAllAsync();

                if (getVeri == null || !getVeri.Any())
                {
                    return NotFound("Kurum bulunamadı.");
                }

                return Ok(getVeri);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }


        }


        // YENİ KURUM KAYDEDER
        [HttpPost("KurumEkle")]
        public async Task<IActionResult> KurumEkle(KurumMuhatapCreateDTO dto)
        {
            try
            {
                await muhatapKurumService.AddAsync(dto);
                return Ok("Kurum başarıyla kaydedildi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }



        [HttpDelete("KurumSil/{id}")]
        public async Task<IActionResult> KurumSil(int id)
        {
            try
            {
                await muhatapKurumService.DeleteAsync(id);
                return Ok("Rota silindi");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("KurumGetir/{id}")]
        public async Task<IActionResult> KurumGetir(int id)
        {
            try
            {
                var gelenVeri = await muhatapKurumService.GetByIdAsync(id);
                if (gelenVeri == null)
                {
                    return NotFound("Böyle bir kurum bulunamadı.");
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
