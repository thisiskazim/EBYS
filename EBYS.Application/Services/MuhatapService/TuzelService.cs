using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Services.MuhatapService
{
    public class TuzelService(IMuhatapRepository muhatapRepository,IMapper mapper): IMuhatapTuzelService
    {
        public async Task AddAsync(TuzelKisiMuhatapCreateDTO createDto)
        {
      
            var entity = mapper.Map<KurumMuhatap>(createDto);

            await muhatapRepository.AddAsync(entity);
            await muhatapRepository.SaveAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<TuzelKisiMuhatapListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TuzelKisiMuhatapUpdateDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TuzelKisiMuhatapUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
