using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.Interface;
using Microsoft.AspNetCore.Http;

namespace EBYS.Persistence.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public int GetKurumId()
        {
            // JWT Token içindeki "KurumId" claim'ini bulur ve döner
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("KurumId")?.Value;
            return string.IsNullOrEmpty(claim) ? 0 : int.Parse(claim);
        }

        public int GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;
            return string.IsNullOrEmpty(claim) ? 0 : int.Parse(claim);
        }
    }
}
