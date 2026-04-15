using EBYS.Application.Common.Interface;
using EBYS.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public string CreateToken(Kullanici kullanici)
        {
            // appsettings.json içerisindeki JwtSettings bölümünü okuyoruz
            var secretKey = _config["JwtSettings:Secret"];
            var issuer = _config["JwtSettings:Issuer"];
            var audience = _config["JwtSettings:Audience"];

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token içerisine gömülecek bilgiler (Claims)
            // KurumId burada mühürlendiği için CurrentUserService bunu okuyabilir.
            var claims = new List<Claim>
            {
                new Claim("UserId", kullanici.Id.ToString()),
                new Claim("BaseKurumId", kullanici.BaseKurumId.ToString()),
                new Claim("RolId", kullanici.RolId.ToString()),
                new Claim("RolAdi", kullanici.Rol.RolAdi),
                new Claim(ClaimTypes.Name, $"{kullanici.Ad} {kullanici.Soyad}"),
                new Claim(ClaimTypes.SerialNumber, kullanici.KimlikNo)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
