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
        public string EvrakSayisi { get; set; } 
        public DateTime EvrakTarihi { get; set; } = DateTime.Now; 
        public string? Olusturan { get; set; }
        public int OlusturanId { get; set; } 
        public string KonuKodu { get; set; }
        public int MuhatapId { get; set; }
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
        public string KayitNo { get; set; } 
        public DateTime DefterTarihi { get; set; }
        public string GonderenMuhatapAdi { get; set; } 
        public bool EditYapabilirMi { get; set; }
        public string SuAnKimde { get; set; }
        public bool IslemSirasiBendeMi { get; set; }
        public int? AlanKullaniciId { get; set; }
        public List<GelenEvrakEkBaseDTO> Ekler { get; set; } = new List<GelenEvrakEkBaseDTO>();
    }


}