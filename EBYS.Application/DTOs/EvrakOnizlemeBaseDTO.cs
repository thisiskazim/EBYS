using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class EvrakOnizlemeBaseDTO
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }
        public byte[] DosyaVerisi { get; set; }
    }



}
