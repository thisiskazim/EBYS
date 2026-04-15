using EBYS.Application.Common.Interface;
using EBYS.Application.DTOs;
using EBYS.Persistence;
using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDTO loginDto)
        {

            // Kullanıcıyı veritabanından bul (Şimdilik düz şifre kontrolü)
            var user = _context.Kullanicilar
                .Include(x => x.Rol)
                .IgnoreQueryFilters()
                .FirstOrDefault(x => x.KimlikNo == loginDto.KimlikNo && x.SifreHash == loginDto.Sifre);

            if (user == null)
                return Unauthorized("Kimlik numarası veya şifre hatalı.");

            // Token üret
            var token = _tokenService.CreateToken(user);

            return Ok(new { Token = token });
        }

        [Authorize] // Sadece giriş yapmış kullanıcılar çıkış yapabilir
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Başarıyla çıkış yapıldı." });
        }
    }
}
