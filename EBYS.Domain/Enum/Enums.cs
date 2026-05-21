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
            GeriIadeEdildi =3,
            HazirSablonOlarakKaydet = 4
            
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

        public enum ImzaTipi
        {
           Paraf=0,
           Imza=1
        }
        public enum  AkisAdimDurumu
        {
            Reddedildi = 0,
            Onaylandi = 1,
            Bekliyor = 2,
            IadeEdildi= 3
        }
        public enum GelenEvrakDurumu
        {
            Kaydedildi = 1,      // Evrak sisteme girildi ama henüz hiçbir yere sevk edilmedi (Boşta)
            SevkEdildi = 2,      // Bir birime veya kişiye gönderildi, alıcının önünde bekliyor
            TeslimAlindi = 3,    // Alıcı personel evrakı kendi üzerine zimmetledi (İşleme aldı)
            IadeEdildi = 4,      // Evrakı sevk eden kişiye geri iade etti
            Cevaplandi = 5       // Evraka karşılık giden evrak yazılarak süreç kapatıldı
        }

    }
}
