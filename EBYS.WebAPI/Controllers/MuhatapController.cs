using AutoMapper;
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
        private readonly IMapper _mapper;

        public MuhatapController(IMuhatapRepository muhatapRepository,EBYSContext context, IMapper mapper)
        {
            _muhatapRepository = muhatapRepository;
            _context = context;
            _mapper = mapper;
        }



        // YENİ KURUM KAYDEDER
        [HttpPost("KurumEkleVeListele")]
        public IActionResult EkleKurum(KurumMuhatapDTO dto)
        {
          var entity = _mapper.Map<KurumMuhatap>(dto);

            _context.Add(entity);
            _context.SaveChanges();
            return Ok();
        }



       
        [HttpPost("VatandasEkleVeListele")]
        public IActionResult EkleBireyselVatandas(BireyselMuhatapDTO dto)
        {
            var entity = _mapper.Map<BireyselMuhatap>(dto);

            _context.Add(entity);
            _context.SaveChanges();
            return Ok();
        }


        [HttpPost("TuzelKisiEkleVeListele")]
        public IActionResult EkleTuzelKisi(TuzelKisiMuhatapDTO dto)
        {
            var entity = _mapper.Map<TuzelKisiMuhatap>(dto);

            _context.Add(entity);
            _context.SaveChanges();
            return Ok();
        }
    }

}
