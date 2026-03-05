using EBYS.Application.Common.Interface;
using EBYS.Application.DTOs;
using EBYS.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBYS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EBYSContext _context;
        private readonly ITokenService _tokenService;

        public AuthController(EBYSContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {


            // Kullanıcıyı veritabanından bul (Şimdilik düz şifre kontrolü)
            var user = _context.Kullanicilar
                .IgnoreQueryFilters()
                .FirstOrDefault(x => x.KimlikNo == loginDto.KimlikNo && x.SifreHash == loginDto.Sifre);

            if (user == null)
                return Unauthorized("Kimlik numarası veya şifre hatalı.");

            // Token üret
            var token = _tokenService.CreateToken(user);

            return Ok(new { Token = token });
        }
    }
}
