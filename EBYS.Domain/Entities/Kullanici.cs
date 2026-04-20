using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Entities
{
    public class Kullanici:BaseEntity
    {
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KimlikNo { get; set; } // E-imza için gerekecek       
        public string SifreHash { get; set; }
        public int RolId { get; set; }
        public virtual Rol Rol { get; set; }

        [NotMapped]
        public string AdSoyad => $"{Ad} {Soyad}"; 
    }

    
}
