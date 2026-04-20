
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Utilities
{
    public class IslemSonuc
    {
        public bool BasariliMi { get; set; }
        public string Mesaj { get; set; }

        public IslemSonuc(bool basariliMi, string mesaj)
        {
            BasariliMi = basariliMi;
            Mesaj = mesaj;
        }

        public static IslemSonuc İslemBasarili(string mesaj = "İşlem başarılı.")
        {
            return new IslemSonuc(true, mesaj);
        }

        public static IslemSonuc Hata(string mesaj)
        {
            return new IslemSonuc(false, mesaj);
        }
    }
}
