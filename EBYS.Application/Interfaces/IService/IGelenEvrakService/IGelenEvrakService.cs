using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Domain.Entities.GelenEvrak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.IService.IGelenEvrakService
{
    public interface IGelenEvrakService:IGenericService<GelenEvrakCreateDTO, GelenEvrakUpdateDTO, GelenEvrakListDTO>
    {
        Task<EvrakOnizlemeBaseDTO> GelenEvrakEkOnizlemeAsync(int ekId);
        Task<List<GelenEvrakSevkListDTO>> GelenEvrakHareketleri(int evrakId);
        Task<GelenEvrakSevk> SahsimaTeslimAl(int evrakId);

    }
}
