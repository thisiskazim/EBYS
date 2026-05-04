using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GidenEvrak
{
    public class GidenEvrakEk:BaseEntity
    {
        public int? EvrakId { get; set; }
        public virtual GidenEvrak? Evrak { get; set; }
        public string? Ad { get; set; }
        public byte[]? DosyaVerisi { get; set; }
        public string? DosyaUzantisi { get; set; }
        public string? MimeType { get; set; }

    }
}
