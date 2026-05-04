using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Services.GelenEvrakService
{
    public class GelenEvrakService : IGelenEvrakService
    {
        public Task AddAsync(GelenEvrakCreateDTO createDto)
        {
           
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GelenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GelenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(GelenEvrakUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
