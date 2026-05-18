using EBYS.Application.DTOs.GelenEvrakDTO;
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

      //  IQueryable<GelenEvrak> GelenEvrakList();

        Task<List<GelenEvrakListDTO>> GelenEvrakListAsync();
        Task<GelenEvrakEk> GelenEvrakEkDosyaByIdAsync(int ekId);

    }
}
