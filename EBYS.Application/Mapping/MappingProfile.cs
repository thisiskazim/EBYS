using AutoMapper;

using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Domain.Entities;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
namespace EBYS.Application.Mapping
{
    public class MappingProfile : Profile//bunların hepsini ayrı ayrı böleceğiz!!!!
    {
        public MappingProfile()
        {
          

            
            // --- ANA LİSTE EKRANI ---
            CreateMap<GidenEvrak, GidenEvrakAkisListeDTO>()
                .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanId))
                .ForMember(dest => dest.OlusturanKullanici, opt => opt.MapFrom(src => src.Olusturan.AdSoyad))
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler))
                .ForMember(dest => dest.SuAnKimde, opt => opt.MapFrom(src => src.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi).Kullanici.AdSoyad ?? "Tamamlandı"))
                .ForMember(dest => dest.FullKonuKodu, opt => opt.MapFrom(src => $"{src.EvrakKonuKodu.KodNumber} - {src.EvrakKonuKodu.KodAdi}"));

            // --- AKIŞ GEÇMİŞİ (Loglar) ---
            CreateMap<GidenEvrakAkis, GidenEvrakAkisHareketleriDTO>()
                .ForMember(dest => dest.KullaniciAdSoyad, opt => opt.MapFrom(src => src.Kullanici.AdSoyad));
            CreateMap<GidenEvrakAkis, AkisAdimDTO>()
                .ForMember(dest => dest.KullaniciAdSoyad, opt => opt.MapFrom(src => src.Kullanici.AdSoyad));


            // --- DÜZENLEME (Edit) FORMU DOLDURMA ---
            // Kullanıcı "Düzenle" dediğinde DB'deki veriyi UpdateDTO'ya doldurur
            CreateMap<GidenEvrak, GidenEvrakUpdateDTO>()
                .ForMember(dest => dest.Ilgiler, opt => opt.MapFrom(src => src.İlgiler))
                .ForMember(dest => dest.Muhataplar, opt => opt.MapFrom(src => src.Muhataplar))
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler));

            // Alt parçaların Entity'den DTO'ya dönüşebilmesi için (Okuma):
            CreateMap<GidenEvrakIlgi, GidenEvrakIlgiUpdateDTO>();
            CreateMap<GidenEvrakEk, GidenEvrakEkUpdateDTO>();
            CreateMap<GidenEvrakMuhatap, GidenEvrakMuhatapSecimDTO>()
                .ForMember(dest => dest.Adi, opt => opt.MapFrom(src => src.Muhatap.Adi));

            // Ekstra: Sadece görüntüleme amaçlı Ek DTO'ları
            CreateMap<GidenEvrakEk, GidenEvrakListDTO>();
            CreateMap<GidenEvrakEk, GidenEvrakEkBaseDTO>();


            CreateMap<GidenEvrakEk, EvrakOnizlemeBaseDTO>();

      


            // 2. YAZMA YÖNÜ (DTO -> Entity) - Ekrandan gelen veriyi DB'ye kaydederken
           

            // --- YENİ KAYIT (Create) ---

            CreateMap<GidenEvrakCreateDTO, GidenEvrak>()
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // --- GÜNCELLEME (Update) ---
           
            CreateMap<GidenEvrakUpdateDTO, GidenEvrak>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // --- ALT LİSTELERİN KAYIT DÖNÜŞÜMLERİ ---
            CreateMap<GidenEvrakIlgiCreateDTO, GidenEvrakIlgi>();
            CreateMap<GidenEvrakIlgiUpdateDTO, GidenEvrakIlgi>();
            CreateMap<GidenEvrakMuhatapSecimDTO, GidenEvrakMuhatap>();

            // Eklerin Kaydı (Dosya verilerini Service'te manuel işlediğimiz için Ignore diyoruz)
            CreateMap<GidenEvrakEkCreateDTO, GidenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore())
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());

            CreateMap<GidenEvrakEkUpdateDTO, GidenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore())
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());

            //////////////////////////////////////GelenEvrak////////////////////////////////////////////////////////



          
            // 1. OKUMA (Entity -> DTO)
      
            CreateMap<GelenEvrak, GelenEvrakListDTO>()
                .ForMember(dest => dest.GonderenMuhatapAdi, opt => opt.MapFrom(src => src.Muhatap.Adi))
                .ForMember(dest => dest.Olusturan, opt => opt.MapFrom(src => src.Olusturan.AdSoyad))
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler))
                .ForMember(dest => dest.AlanKullaniciId, opt => opt.MapFrom(src =>
                            src.Sevkler.OrderByDescending(s => s.SevkTarihi)
                                       .Select(s => s.AlanKullaniciId)
                                       .FirstOrDefault()))

                .ForMember(dest => dest.SuAnKimde, opt => opt.MapFrom(src =>
                            src.Sevkler.OrderByDescending(s => s.SevkTarihi)
                                       .Select(s => s.AlanKullanici.AdSoyad)
                                       .FirstOrDefault() ?? "Sevk Edilmedi"));

            CreateMap<GelenEvrak, GelenEvrakUpdateDTO>()
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler))
                .ForMember(dest => dest.Ilgiler, opt => opt.MapFrom(src => src.Ilgileri));

            // Alt listelerin okunabilmesi için:
            CreateMap<GelenEvrakEk, GelenEvrakEkUpdateDTO>();
            CreateMap<GelenEvrakIlgi, GelenEvrakIlgiUpdateDTO>();
            CreateMap<GelenEvrakEk, GelenEvrakEkBaseDTO>();

            CreateMap<GelenEvrakSevk, GelenEvrakSevkListDTO>()
              
                .ForMember(dest => dest.SevkEdenKullaniciAdSoyad, opt => opt.MapFrom(src => src.SevkEdenKullanici.AdSoyad))
                .ForMember(dest => dest.AlanKullaniciAdSoyad, opt => opt.MapFrom(src => src.AlanKullanici != null ? src.AlanKullanici.AdSoyad : null))
                .ForMember(dest => dest.SevkTarihi, opt => opt.MapFrom(src => src.SevkTarihi))
                .ForMember(dest => dest.Aciklama, opt => opt.MapFrom(src => src.Aciklama));


         
            // 2. YAZMA (DTO -> Entity)
          

            // --- ANA EVRAK KAYIT/GÜNCELLEME ---
           
            CreateMap<GelenEvrakCreateDTO, GelenEvrak>()
                .ForMember(dest => dest.Ekler, opt => opt.Ignore())
                .ForMember(dest => dest.Ilgileri, opt => opt.Ignore())
                .ForMember(dest => dest.Sevkler, opt => opt.Ignore());

            CreateMap<GelenEvrakUpdateDTO, GelenEvrak>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Ekler, opt => opt.Ignore())
                .ForMember(dest => dest.Ilgileri, opt => opt.Ignore())
                .ForMember(dest => dest.Sevkler, opt => opt.Ignore());

            // --- ALT NESNE DÖNÜŞÜMLERİ ---
            
            CreateMap<GelenEvrakEkCreateDTO, GelenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore())
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());

            CreateMap<GelenEvrakEkUpdateDTO, GelenEvrakEk>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore())
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());


            CreateMap<GelenEvrakEk, EvrakOnizlemeBaseDTO>();

            // İLGİ
            CreateMap<GelenEvrakIlgiCreateDTO, GelenEvrakIlgi>();
            CreateMap<GelenEvrakIlgiUpdateDTO, GelenEvrakIlgi>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // SEVK
            CreateMap<GelenEvrakSevkDTO, GelenEvrakSevk>();
            //////////////////////////////////////GelenEvrak END////////////////////////////////////////////////////////



            CreateMap<EvrakKonuKodu, GidenEvrakKonuKoduDTO>()
                .ForMember(dest => dest.FullKod,
                    opt => opt.MapFrom(src => $"{src.KodNumber} - {src.KodAdi}"));

            CreateMap<Muhatap, MuhatapBaseDTO>()
                .Include<KurumMuhatap, KurumMuhatapListDTO>()
                .Include<BireyselMuhatap, BireyselMuhatapListDTO>()
                .Include<TuzelKisiMuhatap, TuzelKisiMuhatapListDTO>();

            CreateMap<KurumMuhatap, KurumMuhatapListDTO>().ReverseMap();
            CreateMap<BireyselMuhatap, BireyselMuhatapListDTO>().ReverseMap();
            CreateMap<TuzelKisiMuhatap, TuzelKisiMuhatapListDTO>().ReverseMap();

            //EKLEME
            CreateMap<KurumMuhatapCreateDTO, KurumMuhatap>().ReverseMap();
            CreateMap<BireyselMuhatapCreateDTO, BireyselMuhatap>().ReverseMap();
            CreateMap<TuzelKisiMuhatapCreateDTO, TuzelKisiMuhatap>().ReverseMap();

            // GÜNCELLEME
            CreateMap<KurumMuhatapUpdateDTO, KurumMuhatap>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<BireyselMuhatapUpdateDTO, BireyselMuhatap>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            CreateMap<TuzelKisiMuhatapUpdateDTO, TuzelKisiMuhatap>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();

            //
            

            //İMZA ROTA 

            CreateMap<ImzaRotaCreateDTO, ImzaRota>()
                .ForMember(dest => dest.ImzaRotaAdimlari, opt => opt.MapFrom(src => src.RotaAdimlari))
                .ReverseMap();


            CreateMap<ImzaRotaUpdateDTO, ImzaRota>()
                .ForMember(dest => dest.ImzaRotaAdimlari, opt => opt.MapFrom(src => src.RotaAdimlari))
                .ReverseMap();


            CreateMap<ImzaRotaAdimlariCreateDTO, ImzaRotaAdimi>().ReverseMap();
            CreateMap<ImzaRotaAdimlariUpdateDTO, ImzaRotaAdimi>().ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<ImzaRota, ImzaRotaListDTO>().ReverseMap();




            CreateMap<Kullanici, KullaniciListDTO>()
                .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => $"{src.Ad} {src.Soyad}"))
                .ForMember(dest => dest.RolAdi, opt => opt.MapFrom(src => src.Rol.RolAdi));

          
            CreateMap<ImzaRotaAdimi, ImzaRotaAdimlariUpdateDTO>()
                .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => $"{src.Kullanici.Ad} {src.Kullanici.Soyad}"))
                .ForMember(dest => dest.RolAdi, opt => opt.MapFrom(src => src.Kullanici.Rol.RolAdi))
                .ForMember(dest => dest.ImzaTuruLabel, opt => opt.MapFrom(src => (int)src.ParafMiImzaMi == 1 ? "İmza" : "Paraf"));



          

        }
    }
}


