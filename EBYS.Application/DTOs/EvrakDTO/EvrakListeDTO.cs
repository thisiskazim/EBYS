using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakListeDTO
    {
        public int Id { get; set; }
        public string OlusturanKullanici { get; set; }
        public int OlusturanKullaniciId { get; set; } // Bunu yetki kontrolü için ekleyelim
        public string Konu { get; set; }
        public string FullKonuKodu { get; set; }
        public DateTime creat_time { get; set; }
        public bool EditYapabilirMi { get; set; } // UI'da butonu gösterip gizlemek için

    }
}
