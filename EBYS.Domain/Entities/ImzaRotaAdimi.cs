using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class ImzaRotaAdimi : BaseEntity
    {
        public int ImzaRotaId { get; set; }
        public ImzaRota Rota { get; set; }
        public int KullaniciId { get; set; }
        public virtual Kullanici Kullanici { get; set; }
        public int SiraNo { get; set; }
        public Enums.ImzaTipi ParafMiImzaMi { get; set; }
    }
}
