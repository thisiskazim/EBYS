using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakAkisHareketleriDTO
    {
        public string KullaniciAdSoyad { get; set; }
        public DateTime creat_time { get; set; }
        public string? Not { get; set; }
            
        // Enum olarak orijinal değer
        public Enums.AkisAdimDurumu AdimDurumu { get; set; }

        // JS tarafında görünecek olan metinsel karşılık
        public string AdimDurumuStr
        {
            get
            {
                return AdimDurumu switch
                {
                    Enums.AkisAdimDurumu.Bekliyor => "Bekliyor",
                    Enums.AkisAdimDurumu.Onaylandi => "Onayladı",
                    Enums.AkisAdimDurumu.Reddedildi => "Reddetti",
                    Enums.AkisAdimDurumu.IadeEdildi => "İade Etti",
                    _ => "Bilinmiyor"
                };
            }
        }
    }
}
