using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;

namespace EBYS.Application.Services
{
    public class KonuKoduService(IGenericRepository<EvrakKonuKodu> konukoduRepository,IMapper mapper) : IKonuKoduService
    {
        public async Task<List<EvrakKonuKoduDTO>> KonuKoduList()
        {
            var get = await konukoduRepository.GetAllAsync();
 
            return mapper.Map<List<EvrakKonuKoduDTO>>(get);
        }
    }
}
