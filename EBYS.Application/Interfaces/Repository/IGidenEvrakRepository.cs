using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IGidenEvrakRepository:IGenericRepository<GidenEvrak>
    {
        Task<List<GidenEvrak>> IslemBekleyenlenKullaniciSorguAsync(int userId,Enums.ImzaTipi imzaTipi);
        Task<GidenEvrak> DetayliGetirAsync(int id);

        Task<List<GidenEvrakAkisListeDTO>> ImzayaGonderdigimEvraklarAsync(int userId);
        Task<List<GidenEvrakAkis>> EvrakHareketleriGetirAsync(int evrakId);
        Task<GidenEvrak> AkisAdimlariSorguAsync(int evrakId);

        Task<GidenEvrakEk> GidenEvrakEkDosyaByIdAsync(int ekId);
        Task<List<GidenEvrakAkisListeDTO>> IslemBekleyenler(int userId,Enums.ImzaTipi imzaTipi);

    }
}
