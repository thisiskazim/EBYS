

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class GidenEvrakAkisListeDTO  //GİDEN EVRAK LİSTE AYNI ZAMANDA
    {
        public int Id { get; set; }
        public string OlusturanKullanici { get; set; }
        public int OlusturanKullaniciId { get; set; } 
        public string Konu { get; set; }
        public string FullKonuKodu { get; set; }
        public string SuAnKimde { get; set; }
        
        public DateTime creat_time { get; set; }
        public bool EditYapabilirMi { get; set; } 
        public bool GeriCekilebilirMi { get; set; }
        public List<GidenEvrakEkBaseDTO> Ekler { get; set; } = new List<GidenEvrakEkBaseDTO>();
        public List<AkisAdimDTO> AkisAdimlari { get; set; } = new List<AkisAdimDTO>();
    }

    public class AkisAdimDTO
    {
        public int KullaniciId { get; set; }
        public int SiraNo { get; set; }
        public bool SiradakiMi { get; set; }
        public string KullaniciAdSoyad { get; set; }
    }
}

