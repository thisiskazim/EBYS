using AutoMapper;

using EBYS.Application.DTOs;
using EBYS.Domain.Entities;
using static EBYS.Domain.Enum.Enums;

namespace EBYS.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
        
            CreateMap<KurumMuhatapDTO, KurumMuhatap>().ReverseMap();
            CreateMap<BireyselMuhatapDTO, BireyselMuhatap>().ReverseMap();
            CreateMap<TuzelKisiMuhatapDTO, TuzelKisiMuhatap>().ReverseMap();

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
            CreateMap<ImzaRotaAdimlariUpdateDTO, ImzaRotaAdimi>().ReverseMap();

            CreateMap<ImzaRota, ImzaRotaListDTO>();


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
