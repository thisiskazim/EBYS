using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrak:BaseEntity
    {
        // Temel Evrak Bilgileri
        public string Konu { get; set; }
        public string EvrakSayisi { get; set; } // Gönderen tarafın sayısı
        public DateTime EvrakTarihi { get; set; } // Gönderen tarafın tarihi
        public DateTime DefterTarihi { get; set; } // Sisteme giriş tarihi (Genelde DateTime.Now)
        public string KayitNo { get; set; } // İçeride takip için (Örn: 2026/1)

        // Tür ve Öncelik
        public bool DilekceMi { get; set; }
        public string KonuKodu { get; set; } // Desimal dosya kodu
        public int GonderenMuhatapId { get; set; } // Rehberden çekilen kurum/şahıs
        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set; }
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set; }

        // Takvim
        public DateTime? CevapIstenenTarih { get; set; }

        // İlişkiler
        public virtual ICollection<GelenEvrakEk> Ekler { get; set; }
        public virtual ICollection<GelenEvrakIlgi> Ilgileri { get; set; }
        public virtual ICollection<GelenEvrakSevk> Sevkler { get; set; }
    }
}
