using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs
{
    public class ImzaRotaBaseDTO
    {
        public string RotaAdi { get; set; }

    }

    public class ImzaRotaCreateDTO : ImzaRotaBaseDTO
    {
        public List<ImzaRotaAdimlariCreateDTO> RotaAdimlari { get; set; }

    }

    public class ImzaRotaUpdateDTO : ImzaRotaBaseDTO
    {
        public int Id { get; set; }
        public List<ImzaRotaAdimlariUpdateDTO> RotaAdimlari { get; set; }

    }

}
