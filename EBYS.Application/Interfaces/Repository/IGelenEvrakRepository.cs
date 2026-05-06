using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IGelenEvrakRepository:IGenericRepository<GelenEvrak>
    {
        Task<int> KayitNumarasiOlustur(int year);
        Task<GelenEvrak> DetayliGetirAsync(int id);
    }
}
