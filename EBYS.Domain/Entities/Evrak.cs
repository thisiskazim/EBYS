using System;
using System.Collections.Generic;
using System.Linq;

using EBYS.Domain.Enum;

namespace EBYS.Domain.Entities
{
    //onaylanma tarihini eklenecek
    //evrak sayısı int olacak


    public class Evrak:BaseEntity
    {
        public string Konu { get; set; }
        public int KonuKoduId { get; set; } = 1;
        public virtual EvrakKonuKodu EvrakKonuKodu { get; set; }

        public string Icerik { get; set; } // Rich Text Editor'den gelen HTML/Text
        public string? ImzaAltindaOlanIcerik { get; set; }
        public int? EvrakSayisi { get; set; }//int olarak değiştirelim.bu belge durumu tamamlandı olduğunda oluşacak bir property.
        public bool IsGelenEvrak { get; set; } // True ise Gelen, False ise Giden
        public Enums.BelgeDurum BelgeDurum { get; set;} // Taslak, İmzada, Tamamlandı vb.

        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set;}
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set;}

        // Navigation Properties (İlişkiler)
        public int OlusturanId { get; private set; }
        public virtual Kullanici Olusturan { get; set; }

        public void SetOlusturanId(int userId) => OlusturanId = userId;

        public int ImzaRotaId { get; set; }
        public virtual ImzaRota ImzaRota { get; set; }

        public virtual ICollection<EvrakMuhatap> Muhataplar { get; set; } = new HashSet<EvrakMuhatap>();
        public virtual ICollection<EvrakIlgi> İlgiler { get; set; } = new HashSet<EvrakIlgi>();
        public virtual ICollection<EvrakEk> Ekler { get; set; } = new HashSet<EvrakEk>();
        public virtual ICollection<EvrakAkis> AkisAdimlari { get; set; } = new HashSet<EvrakAkis>();





    }

   
}
