
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Entities;
using EBYS.Domain.Utilities;

namespace EBYS.Application.Interfaces.IService
{
    public interface IAkisService
    {
        Task<List<EvrakAkisListeDTO>> ImzaBekleyenleriGetirAsync();

    
        Task<List<EvrakAkisListeDTO>> ParafBekleyenleriGetirAsync();

        Task<List<EvrakAkisListeDTO>> ImzayaGonderdigimAsync();

        Task<List<EvrakAkisHareketleriDTO>> EvrakHareketleriGetirAsync(int evrakId);

        Task<IslemSonuc> OnaylaAsync(int evrakId);

        Task<IslemSonuc> GeriCekAsync(int evrakId);

        Task<bool> ReddetAsync(int evrakId, string neden);

        
        Task<bool> IadeEtAsync(int evrakId, string neden);
    }
}
