using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrak:BaseEntity
    {
      
        public string Konu { get; set; }
        public string EvrakSayisi { get; set; } 
        public DateTime EvrakTarihi { get; set; }
        public DateTime DefterTarihi { get; set; } 
        public string KayitNo { get; set; } 
        public bool DilekceMi { get; set; }
        public string KonuKodu { get; set; } 
        public int OlusturanId { get; private set; }
        public virtual Kullanici Olusturan { get; set; }

        public void SetOlusturanId(int userId) => OlusturanId = userId;

        public int MuhatapId { get; set; }
        public virtual Muhatap Muhatap { get; set; }
        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set; }
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set; }
        public DateTime? CevapIstenenTarih { get; set; }
        public virtual ICollection<GelenEvrakEk> Ekler { get; set; }
        public virtual ICollection<GelenEvrakIlgi> Ilgileri { get; set; }
        public virtual ICollection<GelenEvrakSevk> Sevkler { get; set; }
    }
}
