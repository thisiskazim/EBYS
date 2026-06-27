
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Services;
using EBYS.Domain.Exceptions;
using EBYS.Domain.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GidenEvrakController(IGidenEvrakService evrakServive, IKonuKoduService konuKoduService) : ControllerBase
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
            await evrakServive.UpdateAsync(evrakCreateDTO);
            return Ok("Evrak başarıyla güncellendi");
        }

        [HttpDelete("EvrakSil/{id}")]
        public async Task<IActionResult> EvrakSil(int id)
        {

            await evrakServive.DeleteAsync(id);
            return Ok("Evrak silindi");
        }

        [HttpGet("KonuKoduGet")]
        public async Task<IActionResult> KonuKoduGet()
        {
            var konuKodlari = await konuKoduService.KonuKoduList();
            return Ok(konuKodlari);
        }

        [HttpGet("EvrakGetir/{id}")]
        public async Task<IActionResult> EvrakGetirGetById(int id)
        {
            var gelenVeri = await evrakServive.GetByIdAsync(id);

            if (gelenVeri == null)
            {
                 throw new BadRequestException("Böyle bir evrak bulunamadı.");
            }
            return Ok(gelenVeri);
        }
    }
}
