using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.DTOs;
using AutoMapper;
using EBYS.Application.Interfaces.IService;
using EBYS.Domain.Entities;
using EBYS.Application.Interfaces.Repository;




namespace EBYS.Application.Services
{
    public class MuhatapKurumService(IMuhatapRepository kurumRepository,IMapper mapper) : IMuhatapKurumService

    {
      
        public async Task AddKurumAsync(KurumMuhatapDTO kurumDTO)
        {
            var varMi = await kurumRepository.AnyDerivedAsync<KurumMuhatap>(x => x.DetsisNo == kurumDTO.DetsisNo);

            if (varMi)
            {
                throw new InvalidOperationException("DetsisNo değeri zaten mevcut.");
            }
            var entity = mapper.Map<KurumMuhatap>(kurumDTO);

            await kurumRepository.AddAsync(entity);
            await kurumRepository.SaveAsync();
        }


    }
}
