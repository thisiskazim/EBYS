using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class ImzaRotaAdimlariBaseDTO
    {
        public int SiraNo { get; set; }
        public int KullaniciId { get; set; }
        public Enums.ImzaTipi ParafMiImzaMi { get; set; }
    }

    public class ImzaRotaAdimlariCreateDTO:ImzaRotaAdimlariBaseDTO
    {
      
    }

    public class ImzaRotaAdimlariUpdateDTO : ImzaRotaAdimlariBaseDTO
    {
       public int Id { get; set; }
    }

}
