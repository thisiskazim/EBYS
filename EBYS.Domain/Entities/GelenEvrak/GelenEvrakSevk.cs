using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrakSevk:BaseEntity
    {

        public int GelenEvrakId { get; set; }
        public virtual GelenEvrak GelenEvrak { get; set; }
        // Evrakı bir sonraki kişiye sevk eden (Paslayan)
        public int GonderenKullaniciId { get; set; }
        [ForeignKey("GonderenKullaniciId")]
        public virtual Kullanici GonderenKullanici { get; set; }


        public int? AlanKullaniciId { get; set; } // Evrakın gittiği personel

        [ForeignKey("AlanKullaniciId")]
        public virtual Kullanici AlanKullanici { get; set; }

        public string? Aciklama { get; set; } // "Gereği yapılsın", "Bilginize" vb.
        public DateTime SevkTarihi { get; set; }
        public DateTime? OkunmaTarihi { get; set; }
    }
}
