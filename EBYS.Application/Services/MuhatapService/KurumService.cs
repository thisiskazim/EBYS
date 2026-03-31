using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EBYS.Application.Interfaces.IService;
using EBYS.Domain.Entities;
using EBYS.Application.Interfaces.Repository;
using EBYS.Application.DTOs.MuhatapDTO;




namespace EBYS.Application.Services.MuhatapService
{
    public class KurumService(IMuhatapRepository kurumRepository,IMapper mapper) : IMuhatapKurumService

    {
        public async Task AddAsync(KurumMuhatapCreateDTO createDto)
        {
            var varMi = await kurumRepository.AnyDerivedAsync<KurumMuhatap>(x => x.DetsisNo == createDto.DetsisNo);

            if (varMi)
            {
                throw new InvalidOperationException("DetsisNo değeri zaten mevcut.");
            }
            var entity = mapper.Map<KurumMuhatap>(createDto);

            await kurumRepository.AddAsync(entity);
            await kurumRepository.SaveAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<KurumMuhatapListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<KurumMuhatapUpdateDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(KurumMuhatapUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
