using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;

using EBYS.Application.Interfaces.IService.IMuhatapService;

namespace EBYS.Application.Services.MuhatapService
{
    public class TuzelKisiService(IMuhatapRepository tuzelKisiRepository,IMapper mapper): IMuhatapTuzelKisiService
    {
        public async Task AddAsync(TuzelKisiMuhatapCreateDTO createDto)
        {

            var varMi = await tuzelKisiRepository.AnyDerivedAsync<TuzelKisiMuhatap>(x => x.VergiNo == createDto.VergiNo);

            if (varMi)
            {
                throw new InvalidOperationException("Bu vergi numarasına ait kayıt mevcut.");
            }
            var entity = mapper.Map<TuzelKisiMuhatap>(createDto);

            await tuzelKisiRepository.AddAsync(entity);
            await tuzelKisiRepository.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var getVeri = await tuzelKisiRepository.GetByIdAsync(id);
            if (getVeri == null)
            {
                throw new InvalidOperationException("Tüzel Kişi bulunamadı.");
            }
            tuzelKisiRepository.DeleteAsync(getVeri);
            await tuzelKisiRepository.SaveAsync();
        }

        public async Task<List<TuzelKisiMuhatapListDTO>> GetAllAsync()
        {
            var getVeri = await tuzelKisiRepository.GetAllDerivedAsync<TuzelKisiMuhatap>();
            if (getVeri == null)
            {
                throw new InvalidOperationException("Tüzel Kişi Listesi Boş");
            }


            var listDto = mapper.Map<List<TuzelKisiMuhatapListDTO>>(getVeri);
            return listDto;

        }

        public async Task<TuzelKisiMuhatapUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await tuzelKisiRepository.GetByIdAsync(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<TuzelKisiMuhatapUpdateDTO>(getVeri);

            return dto;
        }

        public async Task UpdateAsync(TuzelKisiMuhatapUpdateDTO updateDto)
        {
            var getVeri = await tuzelKisiRepository.GetByIdAsync(updateDto.Id);

            if (getVeri == null)
            {
                throw new Exception("Veri Yok");
            }

            mapper.Map(updateDto, getVeri);


            tuzelKisiRepository.UpdateAsync(getVeri);
            await tuzelKisiRepository.SaveAsync();

        }
    }
}
