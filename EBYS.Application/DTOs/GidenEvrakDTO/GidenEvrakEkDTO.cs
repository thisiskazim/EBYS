using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakEkBaseDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }

    }

    public class EvrakEkCreateDTO
    {
        public string? Ad { get; set; }
        public IFormFile? Dosya { get; set; }
    }

    public class EvrakEkUpdateDTO : EvrakEkBaseDTO
    {
        public IFormFile? Dosya { get; set; }
    }

    public class EvrakEkListDTO : EvrakEkBaseDTO
    {
       

    }
}
