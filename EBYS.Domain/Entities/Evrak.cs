using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class Evrak:BaseEntity
    {
        public string Konu { get; set; }
        public string Icerik { get; set; } // Rich Text Editor'den gelen HTML/Text
        public string EvrakSayisi { get; set; }
        public bool IsGelenEvrak { get; set; } // True ise Gelen, False ise Giden
        public int Durum { get; set; } // Taslak, İmzada, Tamamlandı vb.

        // Navigation Properties (İlişkiler)
        public int OlusturanId { get; set; }
        public virtual Kullanici Olusturan { get; set; }

        public int? BekleyenRolId { get; set; }
        public virtual Rol BekleyenRol { get; set; }

        public virtual ICollection<EvrakMuhatap> Muhataplar { get; set; } = new HashSet<EvrakMuhatap>();
        public virtual ICollection<EvrakIlgi> İlgiler { get; set; } = new HashSet<EvrakIlgi>();
        public virtual ICollection<EvrakEk> Ekler { get; set; } = new HashSet<EvrakEk>();
    }
}
