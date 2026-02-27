using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class MuhatapDTO
    {
        public int Id { get; set; }
        public string Adi { get; set; }
        public string Telefon { get; set; }
        public string? EPosta { get; set; }
        public string Adress { get; set; }
       
    }

    public class KurumMuhatapDTO : MuhatapDTO
    {
        public string KepAdresi { get; set; }
        public string DetsisNo { get; set; }
        public string KurumKodu { get; set; }
    }


    public class TuzelKisiMuhatapDTO : MuhatapDTO
    {
        public string? VergiNo { get; set; }
        public string? VergiDairesi { get; set; }
        public string? MersisNo { get; set; }
    }

    public class BireyselMuhatapDTO : MuhatapDTO
    {
        public string? KimlikNo { get; set; }
    }

}
