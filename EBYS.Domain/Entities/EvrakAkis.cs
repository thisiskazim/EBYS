using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class EvrakAkis : BaseEntity
    {
        public int EvrakId { get; set; }
        public virtual Evrak Evrak { get; set; }

        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }

        public int SiraNo { get; set; }
        public Enums.ImzaTipi ParafMiImzaMi { get; set; } // Paraf mı İmza mı?

        public bool SiradakiMi { get; set; } // "İmza Bekleyenler" listesi için anahtar

        // Kişinin özel durumu
        public Enums.AkisAdimDurumu AdimDurumu { get; set; }
        public string? Not { get; set; } // Red ederse "Eksik bilgi var" gibi not yazması için
    }
}
