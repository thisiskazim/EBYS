using AutoMapper;

using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;

using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;
namespace EBYS.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //EVRAK MAPPİNG
            // --- 1. LİSTELEME EKRANI (Entity -> DTO) ---
            // Evrakları tabloda listelerken kullanılır.
            CreateMap<Evrak, EvrakAkisListeDTO>()
                .ForMember(dest => dest.OlusturanKullaniciId, opt => opt.MapFrom(src => src.OlusturanId))
                .ForMember(dest => dest.OlusturanKullanici, opt => opt.MapFrom(src => src.Olusturan.AdSoyad))
                .ForMember(dest => dest.SuAnKimde, opt => opt.MapFrom(src => src.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi).Kullanici.AdSoyad ?? "Tamamlandı"))
                .ForMember(dest => dest.FullKonuKodu, opt => opt.MapFrom(src => $"{src.EvrakKonuKodu.KodNumber} - {src.EvrakKonuKodu.KodAdi}")
                );


            CreateMap<EvrakAkis, EvrakAkisHareketleriDTO>().ForMember(dest => dest.KullaniciAdSoyad, opt => opt.MapFrom(src => src.Kullanici.AdSoyad));
   

            // --- 2. DÜZENLEME EKRANI (Entity -> DTO) ---

            // Alt listelerin (İlgiler, Muhataplar, Ekler) Entity'den DTO'ya dönüşebilmesi için bunlar ŞART:
            CreateMap<EvrakIlgi, EvrakIlgiUpdateDTO>();
            CreateMap<EvrakMuhatap, EvrakMuhatapSecimDTO>();
            CreateMap<EvrakEk, EvrakEkUpdateDTO>();


            // "Düzenle" butonuna basınca formun ve tabloların dolmasını sağlar.
            CreateMap<Evrak, GidenEvrakUpdateDTO>()
                .ForMember(dest => dest.Ilgiler, opt => opt.MapFrom(src => src.İlgiler))
                .ForMember(dest => dest.Muhataplar, opt => opt.MapFrom(src => src.Muhataplar))
                .ForMember(dest => dest.Ekler, opt => opt.MapFrom(src => src.Ekler));



            // --- 3. KAYDETME VE GÜNCELLEME (DTO -> Entity) ---
            // Ekrandan gelen veriyi veritabanına yazarken kullanılır.

            // Yeni Kayıt
            CreateMap<GidenEvrakCreateDTO, Evrak>()
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // Güncelleme
            CreateMap<GidenEvrakUpdateDTO, Evrak>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore());

            // Alt listelerin DTO'dan Entity'ye dönüşebilmesi için (Service içinde Map yaparken):
            CreateMap<EvrakIlgiCreateDTO, EvrakIlgi>();
            CreateMap<EvrakIlgiUpdateDTO, EvrakIlgi>();
            CreateMap<EvrakEkCreateDTO, EvrakEk>();
            CreateMap<EvrakEkUpdateDTO, EvrakEk>();
            CreateMap<EvrakMuhatapSecimDTO, EvrakMuhatap>();



            CreateMap<EvrakKonuKodu, EvrakKonuKoduDTO>()
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
            CreateMap<EvrakEk, EvrakEkListDTO>();

            // Entity -> EvrakEkBaseDTO (Gerekirse genel kullanım için)
            CreateMap<EvrakEk, EvrakEkBaseDTO>();

            // CreateDTO -> Entity (Kaydetme için)
            CreateMap<EvrakEkCreateDTO, EvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore()) // Dosyayı elle işleyeceğiz
                .ForMember(dest => dest.DosyaUzantisi, opt => opt.Ignore())
                .ForMember(dest => dest.MimeType, opt => opt.Ignore());

            // UpdateDTO -> Entity (Güncelleme için)
            CreateMap<EvrakEkUpdateDTO, EvrakEk>()
                .ForMember(dest => dest.DosyaVerisi, opt => opt.Ignore());

        }
    }
}
