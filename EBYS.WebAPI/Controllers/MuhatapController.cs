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



        // YENİ KURUM KAYDEDER
        [HttpPost("KurumEkleVeListele")]
        public async Task<IActionResult> EkleKurum(KurumMuhatapCreateDTO dto)
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



        //düzenleme yapılacak
        [HttpPost("VatandasEkleVeListele")]
        public async IActionResult EkleBireyselVatandas(BireyselMuhatapCreateDTO dto)
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

        //düzenleme yapılacak
        [HttpPost("TuzelKisiEkleVeListele")]
        public async IActionResult EkleTuzelKisi(TuzelKisiMuhatapCreateDTO dto)
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
    }

}
