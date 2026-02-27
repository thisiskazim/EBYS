using EBYS.Application.DTOs;
using EBYS.Application.Interface;
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
    public class MuhatapController : ControllerBase
    {

        private readonly IMuhatapRepository _muhatapRepository;
        private readonly EBYSContext _context;

        public MuhatapController(IMuhatapRepository muhatapRepository,EBYSContext context)
        {
            _muhatapRepository = muhatapRepository;
            _context = context;
        }

        [HttpGet("GetAlicilar")]
        public async Task<IActionResult> GetAlicilar([FromQuery] DataSourceRequest request)
        {
            // TPH sayesinde Muhataplar tablosundaki Kurum, Birey, Tüzel hepsi tek seferde gelir
            var query = _muhatapRepository.GetReadOnly();

           
            var result = await query.ToDataSourceResultAsync(request);

            return Ok(result);



        }


        // YENİ KURUM KAYDEDER
        [HttpPost("KurumEkleVeListele")]
        public IActionResult EkleKurum(KurumMuhatapDTO dto)
        {
            var entity = new KurumMuhatap
            {
                Adi = dto.Adi,
                DetsisNo = dto.DetsisNo,
                KepAdresi = dto.KepAdresi,
                KurumKodu = dto.KurumKodu,
                Telefon = dto.Telefon,
                Adress = dto.Adress,
                EPosta = dto.EPosta
            };

            _context.Add(entity);
            _context.SaveChanges();
            return Ok();
        }
    }

}
