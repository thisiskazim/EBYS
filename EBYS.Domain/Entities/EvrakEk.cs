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
        public string? EkAdi { get; set; }
     //   public string? DosyaYolu { get; set; } // Fiziksel dosya yolu ekleyeceğimiz zaman kullanacağız

    }
}
