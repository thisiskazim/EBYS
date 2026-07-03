using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class GidenEvrakEkBaseDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }

    }

    public class GidenEvrakEkCreateDTO
    {
        public string? Ad { get; set; }
        public bool IsAsilEvrak { get; set; }

        public IFormFile? Dosya { get; set; }
    }

    public class GidenEvrakEkUpdateDTO : GidenEvrakEkBaseDTO
    {
        public IFormFile? Dosya { get; set; }
        public bool IsAsilEvrak { get; set; }

    }

}
