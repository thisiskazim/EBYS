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
            //EVRAK MAPPİNG
            // --- 1. LİSTELEME EKRANI (Entity -> DTO) ---
            // Evrakları tabloda listelerken kullanılır.
            CreateMap<GidenEvrak, GidenEvrakAkisListeDTO>()
                .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanId))
                .ForMember(dest => dest.OlusturanKullanici, opt => opt.MapFrom(src => src.Olusturan.AdSoyad))
                .ForMember(dest => dest.SuAnKimde, opt => opt.MapFrom(src => src.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi).Kullanici.AdSoyad ?? "Tamamlandı"))
                .ForMember(dest => dest.FullKonuKodu, opt => opt.MapFrom(src => $"{src.EvrakKonuKodu.KodNumber} - {src.EvrakKonuKodu.KodAdi}")
                );

            CreateMap<GidenEvrakAkis, GidenEvrakAkisHareketleriDTO>().ForMember(dest => dest.KullaniciAdSoyad, opt => opt.MapFrom(src => src.Kullanici.AdSoyad));
   
            // --- 2. DÜZENLEME EKRANI (Entity -> DTO) ---

            // Alt listelerin (İlgiler, Muhataplar, Ekler) Entity'den DTO'ya dönüşebilmesi için bunlar ŞART:
            CreateMap<GidenEvrakIlgi, GidenEvrakIlgiUpdateDTO>();

            // Diğer Muhatap -> DTO eşleşmelerini sil, sadece bu kalsın:
            CreateMap<GidenEvrakMuhatap, GidenEvrakMuhatapSecimDTO>()
                .ForMember(dest => dest.MuhatapId, opt => opt.MapFrom(src => src.MuhatapId))
                .ForMember(dest => dest.IsBilgi, opt => opt.MapFrom(src => src.IsBilgi))
                .ForMember(dest => dest.Adi, opt => opt.MapFrom(src => src.Muhatap.Adi));
            CreateMap<GidenEvrakEk, GidenEvrakEkUpdateDTO>();


            // "Düzenle" butonuna basınca formun ve tabloların dolmasını sağlar.
            CreateMap<GidenEvrak, GidenEvrakUpdateDTO>()
                .ForMember(dest => dest.Ilgiler, opt => opt.MapFrom(src => src.İlgiler))
                .ForMember(dest => dest.Muhataplar, opt => opt.MapFrom(src => src.Muhataplar))
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler));


            // --- 3. KAYDETME VE GÜNCELLEME (DTO -> Entity) ---
            // Ekrandan gelen veriyi veritabanına yazarken kullanılır.

            // Yeni Kayıt
            CreateMap<GidenEvrakCreateDTO, GidenEvrak>()
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // Güncelleme
            CreateMap<GidenEvrakUpdateDTO, GidenEvrak>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // Alt listelerin DTO'dan Entity'ye dönüşebilmesi için (Service içinde Map yaparken):
            CreateMap<GidenEvrakIlgiCreateDTO, GidenEvrakIlgi>();
            CreateMap<GidenEvrakIlgiUpdateDTO, GidenEvrakIlgi>();
            CreateMap<GidenEvrakEkCreateDTO, GidenEvrakEk>();
            CreateMap<GidenEvrakEkUpdateDTO, GidenEvrakEk>();
            CreateMap<GidenEvrakMuhatapSecimDTO, GidenEvrakMuhatap>();



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
            // --- ANA EVRAK MAPPING ---
            CreateMap<GelenEvrakCreateDTO, GelenEvrak>();
            CreateMap<GelenEvrakUpdateDTO, GelenEvrak>();

           
            CreateMap<GelenEvrak, GelenEvrakListDTO>();

            // EK 
            CreateMap<GelenEvrakEk, GelenEvrakEkBaseDTO>();

            // Create ve Update'de IFormFile olduğu için AutoMapper bunları 
            // otomatik byte[] dizisine çeviremez. Bu yüzden DosyaVerisi'ni Ignore ediyoruz.
            CreateMap<GelenEvrakEkCreateDTO, GelenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore());

            CreateMap<GelenEvrakEkUpdateDTO, GelenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore());

            // İLGİ 
            CreateMap<GelenEvrakIlgi, GelenEvrakIlgiUpdateDTO>().ReverseMap();
            CreateMap<GelenEvrakIlgiCreateDTO, GelenEvrakIlgi>();

            //  SEVK
            CreateMap<GelenEvrakSevkDTO, GelenEvrakSevk>();  
            CreateMap<GelenEvrakSevk, GelenEvrakSevkListDTO>();


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

            // ImzaRotaAdimi'ndan DTO'ya dönüşürken kuralları tanımlıyoruz
            CreateMap<ImzaRotaAdimi, ImzaRotaAdimlariUpdateDTO>()
                .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => $"{src.Kullanici.Ad} {src.Kullanici.Soyad}"))
                .ForMember(dest => dest.RolAdi, opt => opt.MapFrom(src => src.Kullanici.Rol.RolAdi))
                .ForMember(dest => dest.ImzaTuruLabel, opt => opt.MapFrom(src => (int)src.ParafMiImzaMi == 1 ? "İmza" : "Paraf"));



            // Entity -> EvrakEkListDTO (Görüntüleme için)
            CreateMap<GidenEvrakEk, GidenEvrakListDTO>();

            // Entity -> EvrakEkBaseDTO (Gerekirse genel kullanım için)
            CreateMap<GidenEvrakEk, GidenEvrakEkBaseDTO>();

            // CreateDTO -> Entity (Kaydetme için)
            CreateMap<GidenEvrakEkCreateDTO, GidenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore()) // Dosyayı elle işleyeceğiz
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());

            // UpdateDTO -> Entity (Güncelleme için)
            CreateMap<GidenEvrakEkUpdateDTO, GidenEvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore());

        }
    }
}
