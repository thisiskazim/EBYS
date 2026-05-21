using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.GelenEvrakDTO
{
    public class GelenEvrakSevkDTO //gelen evrak başka birine sevk edilirken 
    {
        public int GelenEvrakId { get; set; } 

        public int SevkEdenKullaniciId { get; set; } 
        public int? AlanKullaniciId { get; set; } 

        public string? Aciklama { get; set; } 
        public DateTime SevkTarihi { get; set; } = DateTime.Now;
        public Enums.GelenEvrakDurumu GelenEvrakDurumEnum { get; set; }
    }
}
