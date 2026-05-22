using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class KullaniciListDTO
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; } 
        public string KimlikNo { get; set; }
        public string RolAdi { get; set; } 
    }
}
