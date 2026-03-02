using AutoMapper;

using EBYS.Application.DTOs;
using EBYS.Domain.Entities;

namespace EBYS.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
        
            CreateMap<KurumMuhatapDTO, KurumMuhatap>().ReverseMap();
            CreateMap<BireyselMuhatapDTO, BireyselMuhatap>().ReverseMap();
            CreateMap<TuzelKisiMuhatapDTO, TuzelKisiMuhatap>().ReverseMap();

        }
    }
}
