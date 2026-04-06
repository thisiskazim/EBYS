using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvtakDTO
{
    public class EvrakIlgiBaseDTO
    {
        public string? IlgiMetni { get; set; }

        //bir gün dosya ekleme ihtiyacı olabilir diye ekledim
       //  public string? EkDosyaYolu { get; set; }
    }

    public class EvrakIlgiCreateDTO : EvrakIlgiBaseDTO
    {
       
    }

    public class EvrakIlgiUpdateDTO : EvrakIlgiBaseDTO
    {
        public int Id { get; set; }
    }


}
