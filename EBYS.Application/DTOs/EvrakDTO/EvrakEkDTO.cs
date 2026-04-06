using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvtakDTO
{
    public class EvrakEkBaseDTO
    {
        public string? EkAdi { get; set; }
      //  public string? DosyaYolu { get; set; } // Fiziksel dosya yolu veya GUID
    }

    public class EvrakEkCreateDTO : EvrakEkBaseDTO
    {

    }

    public class EvrakEkUpdateDTO : EvrakEkBaseDTO
    {
         public int Id { get; set; }
    }
}
