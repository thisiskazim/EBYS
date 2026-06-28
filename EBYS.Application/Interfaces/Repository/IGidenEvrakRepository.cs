using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using static EBYS.Domain.Enum.Enums;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IGidenEvrakRepository:IGenericRepository<GidenEvrak>
    {
      
        Task<GidenEvrak> DetayliGetirAsync(int id);

        Task<List<GidenEvrakAkisListeDTO>> ImzayaGonderdigimEvraklarAsync(int userId);
        Task<List<GidenEvrakAkis>> EvrakHareketleriGetirAsync(int evrakId);
        Task<GidenEvrak> AkisAdimlariSorguAsync(int evrakId);

        Task<GidenEvrakEk> GidenEvrakEkDosyaByIdAsync(int ekId);
        Task<List<GidenEvrakAkisListeDTO>> IslemBekleyenler(int userId,Enums.ImzaTipi imzaTipi);
        Task<List<GidenEvrakAkisListeDTO>> FiltreliEvrakGetirAsync(int? currentUserId, GidenEvrakFiltreTipi? filtreTipi);
   
       
      
        }

}
