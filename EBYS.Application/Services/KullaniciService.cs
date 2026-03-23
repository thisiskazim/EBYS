
using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;


namespace EBYS.Application.Services
{
    public class KullaniciService(IKullaniciRepository kullaniciRepository,IMapper mapper) : IKullaniciService
    {

        public async Task<List<KullaniciListDTO>> GetKullaniciAll()
        {

            var kullanicilar = await kullaniciRepository.AsyncKullanicilarVeRolleriniGetir();


            return mapper.Map<List<KullaniciListDTO>>(kullanicilar);
        }

    }
}
