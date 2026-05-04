using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Entities;
using EBYS.Domain.Utilities;

namespace EBYS.Application.Interfaces.IService.IGidenEvrakService.IGidenEvrakService
{
    public interface IGidenEvrakAkisService
    {
        Task<List<GidenEvrakAkisListeDTO>> ImzaBekleyenleriGetirAsync();

    
        Task<List<GidenEvrakAkisListeDTO>> ParafBekleyenleriGetirAsync();

        Task<List<GidenEvrakAkisListeDTO>> ImzayaGonderdigimAsync();

        Task<List<GidenEvrakAkisHareketleriDTO>> EvrakHareketleriGetirAsync(int evrakId);

        Task<IslemSonuc> OnaylaAsync(int evrakId);

        Task<IslemSonuc> GeriCekAsync(int evrakId);

        Task<bool> ReddetAsync(int evrakId, string neden);

        
        Task<bool> IadeEtAsync(int evrakId, string neden);
    }
}
