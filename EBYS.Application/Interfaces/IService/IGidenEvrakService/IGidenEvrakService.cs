using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;


namespace EBYS.Application.Interfaces.IService.IGidenEvrakService
{
    public interface IGidenEvrakService:IGenericService<GidenEvrakCreateDTO, GidenEvrakUpdateDTO, GidenEvrakListDTO>
    {
        Task<EvrakOnizlemeBaseDTO> GidenEvrakEkOnizlemeAsync(int ekId);

    }
}
