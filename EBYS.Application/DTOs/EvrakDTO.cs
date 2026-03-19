using EBYS.Domain.Entities;
using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class GidenEvrakBaseDTO
    
    {
        public string Konu { get; set; }
        public string Icerik { get; set; } // Rich Text Editor'den gelen HTML/Text
        public string? ImzaAltindaOlanIcerik { get; set; }
        //public string EvrakSayisi { get; set; }//bu belge durumu tamamlandı olduğunda oluşacak bir property
        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set; }
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set; }
        public int ImzaRotaId { get; set; }
        public List<int> MuhatapIds { get; set; } = new();
        public List<int> IlgiEvrakIds { get; set; } = new();
        public List<int> EkIds { get; set; } = new();

    }

    public class GidenEvrakCreateDTO : GidenEvrakBaseDTO
    {

    }
    public class GidenEvrakUpdateDTO : GidenEvrakBaseDTO
    {
        public int Id { get; set; }
    }
}
