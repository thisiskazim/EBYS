    using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.GelenEvrakDTO
{
    public class GelenEvrakBaseDTO
    {
        public string Konu { get; set; }
        public string EvrakSayisi { get; set; } // Gönderen tarafın sayısı
        public DateTime EvrakTarihi { get; set; } = DateTime.Now; // Gönderen tarafın tarihi
        public DateTime DefterTarihi { get; set; } = DateTime.Now;
        public string? Olusturan { get; set; }
        public int OlusturanId { get; set; } // Bunu yetki kontrolü için ekleyelim

        public string KonuKodu { get; set; } // Desimal kod
        public int MuhatapId { get; set; } // Rehberden seçilen kurum/şahıs
        public bool DilekceMi { get; set; }
        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set; }
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set; }
        public DateTime? CevapIstenenTarih { get; set; }
    }

    public class GelenEvrakCreateDTO : GelenEvrakBaseDTO
    {

        public List<GelenEvrakIlgiCreateDTO>? Ilgiler { get; set; } = new();
        public List<GelenEvrakEkCreateDTO>? Ekler { get; set; } = new();
    }

    public class GelenEvrakUpdateDTO : GelenEvrakBaseDTO
    {
        public int Id { get; set; }
        public List<GelenEvrakIlgiUpdateDTO>? Ilgiler { get; set; } = new();
        public List<GelenEvrakEkUpdateDTO>? Ekler { get; set; } = new();


    }

    public class GelenEvrakListDTO : GelenEvrakBaseDTO
    {
        public int Id { get; set; }
        public string KayitNo { get; set; } // Bizim verdiğimiz takip no (2026/1 gibi)
        public DateTime DefterTarihi { get; set; }
        public string GonderenMuhatapAdi { get; set; } // UI'da ID yerine isim göstermek için
        public bool EditYapabilirMi { get; set; }


        // Evrakın o an kimin üzerinde olduğunu göstermek için
        public string SuAnKimde { get; set; }

        public List<GelenEvrakEkBaseDTO> Ekler { get; set; } = new List<GelenEvrakEkBaseDTO>();
    }


}