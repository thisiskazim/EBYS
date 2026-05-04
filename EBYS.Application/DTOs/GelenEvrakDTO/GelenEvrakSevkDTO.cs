using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.GelenEvrakDTO
{
    public class GelenEvrakSevkDTO
    {
        public int GelenEvrakId { get; set; } // Hangi evrak sevk ediliyor?

        public int KaydedenKullaniciId { get; set; } // Sevk eden kullanıcı
        public int? AlanKullaniciId { get; set; } // Sevk edilen kullanıcı

        public string? Aciklama { get; set; } // "Gereği yapılsın", "Arşivlensin" gibi notlar
        public DateTime SevkTarihi { get; set; } = DateTime.Now;
    }
}
