using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;

namespace EBYS.Application.Services.MuhatapService
{
    public class BireyselService(IMuhatapRepository bireyselRepository, IMapper mapper) : IMuhatapBireyselService
    {
        public async Task AddAsync(BireyselMuhatapCreateDTO createDto)
        {
            var varMi = await bireyselRepository.AnyDerivedAsync<BireyselMuhatap>(x => x.KimlikNo == createDto.KimlikNo);

            if (varMi)
            {
                throw new InvalidOperationException("Bu kimlik numarasına ait kayıt mevcut.");
            }
            var entity = mapper.Map<BireyselMuhatap>(createDto);

            await bireyselRepository.AddAsync(entity);
            await bireyselRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var getVeri = await bireyselRepository.GetByIdAsync(id);
            if (getVeri == null)
            {
                throw new InvalidOperationException("Vatandaş Muhatap bulunamadı.");
            }
            bireyselRepository.Delete(getVeri);
            await bireyselRepository.SaveAsync();
        }

        public async Task<List<BireyselMuhatapListDTO>> GetAllAsync()
        {
            var getVeri = await bireyselRepository.GetAllDerivedAsync<BireyselMuhatap>();
            if (getVeri == null)
            {
                throw new InvalidOperationException("Vatandaş Listesi Boş");
            }

            var listDto = mapper.Map<List<BireyselMuhatapListDTO>>(getVeri);
            return listDto;
        }

        public async Task<BireyselMuhatapUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await bireyselRepository.GetByIdAsync(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<BireyselMuhatapUpdateDTO>(getVeri);

            return dto;
        }

        public async Task UpdateAsync(BireyselMuhatapUpdateDTO updateDto)
        {
            var getVeri = await bireyselRepository.GetByIdAsync(updateDto.Id);

            if (getVeri == null)
            {
                throw new Exception("Veri Yok");
            }

            mapper.Map(updateDto, getVeri);

            bireyselRepository.UpdateAsync(getVeri);
            await bireyselRepository.SaveAsync();
        }
    }
}
