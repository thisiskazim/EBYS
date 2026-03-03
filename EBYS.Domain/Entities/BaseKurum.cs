using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class BaseKurum: BaseEntity
    {
      
        public string KurumAdi { get; set; }
        public string VergiNo { get; set; }
        public string KurumKodu { get; set; } 
        public string DetsisNo { get; set; }
        public bool IsActive { get; set; } = true;
        public virtual ICollection<Kullanici> Kullanicilar { get; set; }
    }
}
    