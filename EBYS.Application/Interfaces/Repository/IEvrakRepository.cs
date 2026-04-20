
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IEvrakRepository:IGenericRepository<Evrak>
    {
        Task<List<Evrak>> IslemBekleyenlenKullaniciSorguAsync(int userId,Enums.ImzaTipi imzaTipi);
        Task<Evrak> DetayliGetirAsync(int id);

        Task<List<Evrak>> ImzayaGonderdigimEvraklarAsync(int userId);
        Task<List<EvrakAkis>> EvrakHareketleriGetirAsync(int evrakId);
        Task<Evrak> AkisAdimlariSorguAsync(int evrakId);



    }
}
