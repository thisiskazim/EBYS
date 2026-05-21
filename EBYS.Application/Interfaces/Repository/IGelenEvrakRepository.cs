using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EBYS.Domain.Enum.Enums;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IGelenEvrakRepository:IGenericRepository<GelenEvrak>
    {
        Task<int> KayitNumarasiOlustur(int year);
        Task<GelenEvrak> DetayliGetirByIdAsync(int id);
        Task<List<GelenEvrakListDTO>> FiltreliEvrakGetirAsync(int? currentUserId, GelenEvrakDurumu? evrakDurumu);
        Task<GelenEvrakEk> GelenEvrakEkDosyaByIdAsync(int ekId);
        Task<List<GelenEvrakSevkListDTO>> GelenEvrakSevkHareketleriAsync(int gelenEvrakId);
        Task<GelenEvrakSevk> SevkGetirByIdAsync(int gelenEvrakId);

    }
}
