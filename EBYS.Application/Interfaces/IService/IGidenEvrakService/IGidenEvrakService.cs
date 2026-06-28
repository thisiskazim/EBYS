using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;


namespace EBYS.Application.Interfaces.IService.IGidenEvrakService
{
    public interface IGidenEvrakService:IGenericService<GidenEvrakCreateDTO, GidenEvrakUpdateDTO, GidenEvrakListDTO>
    {
        Task<EvrakOnizlemeBaseDTO> GidenEvrakEkOnizlemeAsync(int ekId);
        Task<List<GidenEvrakAkisListeDTO>> GidenEvraklariFiltreliListeleAsync(Enums.GidenEvrakFiltreTipi? filtreTipi);
 
    }
}
