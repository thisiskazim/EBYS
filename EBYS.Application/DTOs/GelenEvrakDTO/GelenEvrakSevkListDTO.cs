using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.GelenEvrakDTO
{
    public class GelenEvrakSevkListDTO //sevk akışını göstermek için
    {
        public int GelenEvrakId { get; set; }

        // Kimden geldi?
        public string SevkEdenKullaniciAdSoyad { get; set; }

        // Kime gitti?
        public string? AlanKullaniciAdSoyad { get; set; }

        // Ne zaman yapıldı?
        public DateTime SevkTarihi { get; set; }

        // Notu ne?
        public string? Aciklama { get; set; }

        // Ekstra: Hareket Tipi (Opsiyonel)
        // "Sevk Edildi", "Teslim Alındı" gibi metinler basabilirsin.
    }
}
