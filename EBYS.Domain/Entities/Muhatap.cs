using EBYS.Domain.Entities.GidenEvrak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class Muhatap:BaseEntity
    {
        public string Adi { get; set; } 
        public string Telefon { get; set; }
        public string? EPosta { get; set; }
        public string Adress { get; set; }
            
        public virtual ICollection<GidenEvrakMuhatap> Evraklar { get; set; } = new HashSet<GidenEvrakMuhatap>();
    }


    public class KurumMuhatap : Muhatap
    {
        public string KepAdresi { get; set; }
        public string? TesisNo { get; set; }
        public string DetsisNo { get; set; } 
        public string KurumKodu { get; set; }
    }
    public class TuzelKisiMuhatap : Muhatap
    {
        public string? VergiNo { get; set; }
        public string? VergiDairesi { get; set; }
        public string? MersisNo { get; set; }
    }

    public class BireyselMuhatap : Muhatap
    {
        public string? KimlikNo { get; set; }
    }


}