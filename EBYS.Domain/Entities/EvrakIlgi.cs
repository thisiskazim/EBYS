using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class EvrakIlgi:BaseEntity
    {
        public int? EvrakId { get; set; }
        public virtual Evrak? Evrak { get; set; }
        public string? IlgiMetni { get; set; } // Örn: 12.01.2024 tarihli ve 123 sayılı yazı.
    }
}
