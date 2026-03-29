using EBYS.Application.DTOs;
using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.IService
{
    public interface IImzaRotaService
    {
        Task AddImzaRotaAsync(ImzaRotaCreateDTO imzaRotaDto);
        Task UpdateImzaRotaAsync(ImzaRotaUpdateDTO imzaRotaDto);
        Task DeleteImzaRotaAsync(int id);

        Task<ImzaRotaUpdateDTO> ImzaRotaGetByIdAsycn(int id);

        Task<List<ImzaRotaListDTO>> ImzaRotaListAsync();

    }
}
