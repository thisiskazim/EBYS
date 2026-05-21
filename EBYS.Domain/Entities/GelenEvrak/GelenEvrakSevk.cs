using EBYS.Domain.Enum;
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
        public int SevkEdenKullaniciId { get; set; }
        [ForeignKey("SevkEdenKullaniciId")]
        public virtual Kullanici SevkEdenKullanici { get; set; }

        public int? AlanKullaniciId { get; set; } 

        [ForeignKey("AlanKullaniciId")]
        public virtual Kullanici AlanKullanici { get; set; }

        public Enums.GelenEvrakDurumu GelenEvrakDurumEnum { get; set; } = Enums.GelenEvrakDurumu.Kaydedildi;
        public string? Aciklama { get; set; } 
        public DateTime SevkTarihi { get; set; }
        public DateTime? OkunmaTarihi { get; set; }
    }
}
