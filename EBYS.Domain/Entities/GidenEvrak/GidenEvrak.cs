using System;
using System.Collections.Generic;
using System.Linq;
using EBYS.Domain.Enum;

namespace EBYS.Domain.Entities.GidenEvrak
{
    //onaylanma tarihini eklenecek


    public class GidenEvrak:BaseEntity
    {
        public string Konu { get; set; }
        public int KonuKoduId { get; set; } = 1;
        public virtual EvrakKonuKodu EvrakKonuKodu { get; set; }

        public string Icerik { get; set; } 
        public string? ImzaAltindaOlanIcerik { get; set; }
        public int? EvrakSayisi { get; set; }
        public bool IsGelenEvrak { get; set; } 
        public Enums.GidenEvrakDurum BelgeDurum { get; set;} 

        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set;}
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set;}


        public int OlusturanId { get; private set; }
        public virtual Kullanici Olusturan { get; set; }

        public void SetOlusturanId(int userId) => OlusturanId = userId;

        public int ImzaRotaId { get; set; }
        public virtual ImzaRota ImzaRota { get; set; }

        public virtual ICollection<GidenEvrakMuhatap> Muhataplar { get; set; } = new HashSet<GidenEvrakMuhatap>();
        public virtual ICollection<GidenEvrakIlgi> İlgiler { get; set; } = new HashSet<GidenEvrakIlgi>();
        public virtual ICollection<GidenEvrakEk> Ekler { get; set; } = new HashSet<GidenEvrakEk>();
        public virtual ICollection<GidenEvrakAkis> AkisAdimlari { get; set; } = new HashSet<GidenEvrakAkis>();
    }

   
}
