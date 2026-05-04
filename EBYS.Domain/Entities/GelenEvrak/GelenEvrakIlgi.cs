using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities.GelenEvrak
{
    public class GelenEvrakIlgi:BaseEntity
    {
        public int? GelenEvrakId { get; set; }
        public virtual GelenEvrak? GelenEvrak { get; set; }

        public string? IlgiMetni { get; set; }
    }
}
