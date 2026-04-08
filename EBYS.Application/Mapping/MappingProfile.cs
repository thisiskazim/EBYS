using AutoMapper;

using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.EvtakDTO;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Domain.Entities;
namespace EBYS.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {

            //1. LİSTELEME (Polimorfizm/Kalıtım Yönetimi)
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

            CreateMap<EvrakIlgiCreateDTO, EvrakIlgi>();
            CreateMap<EvrakEkCreateDTO, EvrakEk>();
            CreateMap<GidenEvrakCreateDTO, Evrak>();



            // Create senaryosu için
            CreateMap<ImzaRotaCreateDTO, ImzaRota>()
                .ForMember(dest => dest.ImzaRotaAdimlari, opt => opt.MapFrom(src => src.RotaAdimlari))
                .ReverseMap();

            // Update senaryosu için
            CreateMap<ImzaRotaUpdateDTO, ImzaRota>()
                .ForMember(dest => dest.ImzaRotaAdimlari, opt => opt.MapFrom(src => src.RotaAdimlari))
                .ReverseMap();

            // Alt adımlar için (İsimler aynıysa ReverseMap yeterli)
            CreateMap<ImzaRotaAdimlariCreateDTO, ImzaRotaAdimi>().ReverseMap();
            CreateMap<ImzaRotaAdimlariUpdateDTO, ImzaRotaAdimi>().ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ImzaRota, ImzaRotaListDTO>().ReverseMap();


            CreateMap<GidenEvrakCreateDTO, Evrak>()
                // Zaten private set ama niyetimizi belli etmek için ignore ediyoruz
                .ForMember(dest => dest.OlusturanId, opt => opt.Ignore())
                .ForMember(dest => dest.BaseKurumId, opt => opt.Ignore())
                .ForMember(dest => dest.BelgeDurum, opt => opt.Ignore())

                // Bunlar Service katmanında manuel doldurulacak (İlişkili tablolar)
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore())
                .ForMember(dest => dest.AkisAdimlari, opt => opt.Ignore());


            CreateMap<GidenEvrakUpdateDTO, Evrak>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OlusturanId, opt => opt.Ignore())
                .ForMember(dest => dest.BaseKurumId, opt => opt.Ignore())
                .ForMember(dest => dest.BelgeDurum, opt => opt.Ignore())
                    // KOLEKSİYONLAR: Bunlar bizim "dokunulmaz" alanlarımız.
                .ForMember(dest => dest.Muhataplar, opt => opt.Ignore())
                .ForMember(dest => dest.AkisAdimlari, opt => opt.Ignore())
                .ForMember(dest => dest.Ekler, opt => opt.Ignore())
                .ForMember(dest => dest.İlgiler, opt => opt.Ignore());

            CreateMap<Kullanici, KullaniciListDTO>()
                .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => $"{src.Ad} {src.Soyad}"))
                .ForMember(dest => dest.RolAdi, opt => opt.MapFrom(src => src.Rol.RolAdi));

            // ImzaRotaAdimi'ndan DTO'ya dönüşürken kuralları tanımlıyoruz
            CreateMap<ImzaRotaAdimi, ImzaRotaAdimlariUpdateDTO>()
                .ForMember(dest => dest.AdSoyad, opt => opt.MapFrom(src => $"{src.Kullanici.Ad} {src.Kullanici.Soyad}"))
                .ForMember(dest => dest.RolAdi, opt => opt.MapFrom(src => src.Kullanici.Rol.RolAdi))
                .ForMember(dest => dest.ImzaTuruLabel, opt => opt.MapFrom(src => (int)src.ParafMiImzaMi == 1 ? "İmza" : "Paraf"));

        }
    }
}
