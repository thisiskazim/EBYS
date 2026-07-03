using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GidenEvrak
{
    public class GidenEvrakAkis : BaseEntity
    {
        /// <summary>Rota adımları bu değerin altında; durum değişikliği hareket kayıtları bu değer ve üzerinde tutulur.</summary>
        public const int HareketBaslangicSiraNo = 1000;

        public int EvrakId { get; set; }
        public virtual GidenEvrak Evrak { get; set; }

        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }

        public int SiraNo { get; set; }
        public Enums.ImzaTipi ParafMiImzaMi { get; set; } 

        public bool SiradakiMi { get; set; } 
        public Enums.AkisAdimDurumu AdimDurumu { get; set; }
        public string? Not { get; set; } 
    }
}
