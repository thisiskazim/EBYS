using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakListeDTO
    {
        public string OlusturanKullanici { get; set; }
        public string EvrakKonu { get; set; }
        public string FullKonuKodu { get; set; }
        public DateTime Creat_time { get; set; }

    }
}
