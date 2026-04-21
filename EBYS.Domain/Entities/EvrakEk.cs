using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class EvrakEk:BaseEntity
    {
        public int? EvrakId { get; set; }
        public virtual Evrak? Evrak { get; set; }
        public string? Ad { get; set; }
        public byte[]? DosyaVerisi { get; set; }
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }

    }
}
