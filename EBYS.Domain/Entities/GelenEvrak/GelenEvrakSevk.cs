using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrakSevk:BaseEntity
    {

        public int GelenEvrakId { get; set; }
        public virtual GelenEvrak GelenEvrak { get; set; }
        public int GonderenUserId { get; set; } // Evrakı paslayan (veya kaydeden memur)
        public int? AliciUserId { get; set; } // Evrakın gittiği personel

        public string Aciklama { get; set; } // "Gereği yapılsın", "Bilginize" vb.
        public DateTime SevkTarihi { get; set; }
        public DateTime? OkunmaTarihi { get; set; }
    }
}
