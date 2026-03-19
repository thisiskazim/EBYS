using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class ImzaRota:BaseEntity
    {
        public string RotaAdi { get; set; }
        public ICollection<ImzaRotaAdimi> ImzaRotaAdimlari { get; set; }
    }
}
