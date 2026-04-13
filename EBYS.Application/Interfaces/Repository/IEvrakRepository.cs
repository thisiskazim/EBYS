
using EBYS.Domain.Entities;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IEvrakRepository:IGenericRepository<Evrak>
    {
        Task<List<Evrak>> ImzaBekleyenlenKullaniciSorgu(int userId);
        Task<Evrak> DetayliGetirAsync(int id);

    }
}
