using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GidenEvrak
{
    public class GidenEvrakIlgi:BaseEntity
    {
        public int? EvrakId { get; set; }
        public virtual GidenEvrak? Evrak { get; set; }
        public string? IlgiMetni { get; set; } 

        // fiziksel dosya ekleme ihtiyacı olabilir diye ekledim, ama şimdilik kullanmayacağız
        // public int? ReferansEvrakId { get; set; } 
        // public string? DosyaYolu { get; set; } 
    }
}

