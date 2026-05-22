using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrakEk:BaseEntity
    {
        public int? GelenEvrakId { get; set; } 
        public virtual GelenEvrak? GelenEvrak { get; set; }

        public string? Ad { get; set; }
        public byte[]? DosyaVerisi { get; set; } 
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }
    }
}
