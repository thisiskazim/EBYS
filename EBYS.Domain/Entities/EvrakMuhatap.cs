using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class EvrakMuhatap: BaseEntity
    {
        public int EvrakId { get; set; }
        public virtual Evrak Evrak { get; set; }


        public int MuhatapId { get; set; }
        public virtual Muhatap Muhatap { get; set; }

        public bool IsBilgi { get; set; } // Gereği mi yoksa Bilgi mi?
    }
}
