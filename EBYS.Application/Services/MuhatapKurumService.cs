using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.DTOs;
using AutoMapper;
using EBYS.Application.Interface;
using EBYS.Domain.Entities;



namespace EBYS.Application.Services
{
    public class MuhatapKurumService
    {
        private readonly IGenericRepository<KurumMuhatap> _kurumRepository;
        private readonly IMapper _mapper;

        public MuhatapKurumService(IGenericRepository<KurumMuhatap> kurumRepository)
        {
            _kurumRepository = kurumRepository;
        }

        public async Task AddKurumAsync(KurumMuhatapDTO kurumDTO)
        {
            var entity = _mapper.Map<KurumMuhatap>(kurumDTO);

            if (kurumDTO.DetsisNo == entity.DetsisNo)
            {
                throw new InvalidOperationException("DetsisNo değeri zaten mevcut.");
            }

            await _kurumRepository.AddAsync(entity);
            await _kurumRepository.SaveAsync();
        }


    }
}
