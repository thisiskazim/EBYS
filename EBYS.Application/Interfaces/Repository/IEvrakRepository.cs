
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IEvrakRepository:IGenericRepository<Evrak>
    {
        Task<List<Evrak>> IslemBekleyenlenKullaniciSorgu(int userId,Enums.ImzaTipi imzaTipi);
        Task<Evrak> DetayliGetirAsync(int id);

        Task<Evrak> AkisAdimlariSorgu(int evrakId);
    }
}
