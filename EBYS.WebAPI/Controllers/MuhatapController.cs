using EBYS.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI; // Kendo paketini kurmuş olmalısın
using Kendo.Mvc.Extensions;

namespace EBYS.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuhatapController : ControllerBase
    {

        private readonly IMuhatapRepository _muhatapRepository;

        public MuhatapController(IMuhatapRepository muhatapRepository)
        {
            _muhatapRepository = muhatapRepository;
        }

        [HttpGet("GetAlicilar")]
        public async Task<IActionResult> GetAlicilar([DataSourceRequest] DataSourceRequest request)
        {
            // TPH sayesinde Muhataplar tablosundaki Kurum, Birey, Tüzel hepsi tek seferde gelir
            var query = _muhatapRepository.GetReadOnly();

            // ToDataSourceResultAsync: Kendo'nun gönderdiği filtreleri (Arama kelimesi vb.) 
            // SQL'e çevirir ve sadece gereken veriyi döner.
            var result = await query.ToDataSourceResultAsync(request);

            return Ok(result);



        }
    }

}
