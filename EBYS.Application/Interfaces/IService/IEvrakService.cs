using EBYS.Application.DTOs.EvrakDTO;


namespace EBYS.Application.Interfaces.IService
{
    public interface IEvrakService:IGenericService<GidenEvrakCreateDTO, GidenEvrakUpdateDTO, GidenEvrakListDTO>
    {
        Task<List<EvrakListeDTO>> ImzaBekleyenListe();
        Task<List<EvrakListeDTO>> ParafBekleyenListe();
    }
}
