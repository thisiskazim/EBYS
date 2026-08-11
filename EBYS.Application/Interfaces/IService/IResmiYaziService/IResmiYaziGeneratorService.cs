using EBYS.Application.DTOs.ResmiYaziDTO;

namespace EBYS.Application.Interfaces.IService.IResmiYaziService
{
    public interface IResmiYaziGeneratorService
    {
        Task<ResmiYaziGenerateResponse> ResmiMetinOlusturAsync(ResmiYaziGenerateRequest request);
    }
}
