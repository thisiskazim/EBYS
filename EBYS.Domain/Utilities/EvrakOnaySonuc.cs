
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Utilities
{
    public class EvrakOnaySonuc
    {
        public bool BasariliMi { get; set; }
        public string Mesaj { get; set; }
        public EvrakOnaySonuc(bool basariliMi, string mesaj)
        {
            BasariliMi = basariliMi;
            Mesaj = mesaj;
        }
    }
}
