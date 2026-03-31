using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.MuhatapDTO
{
    public class MuhatapBaseDTO
    {
        public string Adi { get; set; }
        public string Telefon { get; set; }
        public string? EPosta { get; set; }
        public string Adress { get; set; }
       
    }

    public class KurumMuhatapBaseDTO : MuhatapBaseDTO
    {
        public string KepAdresi { get; set; }
        public string DetsisNo { get; set; }
        public string KurumKodu { get; set; }
    }


    public class TuzelKisiMuhatapBaseDTO : MuhatapBaseDTO
    {
        public string? VergiNo { get; set; }
        public string? VergiDairesi { get; set; }
        public string? MersisNo { get; set; }
    }

    public class BireyselMuhatapBaseDTO : MuhatapBaseDTO
    {
        public string? KimlikNo { get; set; }
    }

}
