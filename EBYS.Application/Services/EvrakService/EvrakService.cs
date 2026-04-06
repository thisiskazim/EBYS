using AutoMapper;
using EBYS.Application.DTOs.EvtakDTO;
using EBYS.Application.Interfaces.IService.IEvrakService;
using EBYS.Application.Interfaces.Repository;

namespace EBYS.Application.Services.EvrakService
{
    public class EvrakService(IEvrakRepository evrakRepository,IMapper mapper) : IEvrakService

    {
        public Task AddAsync(GidenEvrakCreateDTO createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GidenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GidenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(GidenEvrakUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
