using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Enum
{
    public class Enums
    {
        public enum MuhatapTipi
        {
            Kurum = 1,
            TuzelKisi = 2,
            Bireysel = 3
        }
       public enum BelgeDurum
        {
            Taslak = 0,
            Imzada = 1,
            Tamamlandi = 2,
            GeriIadeEdildi = 3
        }
        public enum GizlilikDerecesi
        {
            Normal = 0,
            TasnifDisi = 1,
            Gizli = 2,
            CokGizli = 3
        }
        public enum IvedilikDerecesi
        {
            Normal = 0,
            Ivedi = 1,
            CokIvedi = 2,
            Gunlu = 3
        }

    }
}
