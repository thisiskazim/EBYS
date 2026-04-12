using EBYS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.DTOs.EvrakDTO;

namespace EBYS.Application.Interfaces.IService
{
    public interface IKonuKoduService
    {
        Task<List<EvrakKonuKoduDTO>> KonuKoduList();
    }
}
