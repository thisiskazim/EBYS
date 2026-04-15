
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Domain.Utilities;

namespace EBYS.Application.Interfaces.IService
{
    public interface IAkisService
    {
        Task<List<EvrakAkisListeDTO>> ImzaBekleyenleriGetirAsync();

    
        Task<List<EvrakAkisListeDTO>> ParafBekleyenleriGetirAsync();

        Task<EvrakOnaySonuc> OnaylaAsync(int evrakId);

       
        Task<bool> ReddetAsync(int evrakId, string neden);

        
        Task<bool> IadeEtAsync(int evrakId, string neden);
    }
}
