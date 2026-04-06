using AutoMapper;
using EBYS.Application.DTOs.MuhatapDTO;
using EBYS.Application.Interfaces.IService.IMuhatapService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;




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

        public async Task DeleteAsync(int id)
        {
            var getVeri = await kurumRepository.GetByIdAsync(id);   
            if (getVeri == null)
            {
                throw new InvalidOperationException("Kurum Muhatap bulunamadı.");
            }
            kurumRepository.DeleteAsync(getVeri);
            await kurumRepository.SaveAsync();

        }

        public async Task<List<KurumMuhatapListDTO>> GetAllAsync()
        {
            var getVeri = await kurumRepository.GetAllDerivedAsync<KurumMuhatap>();
            if (getVeri ==null)
            {
                throw new InvalidOperationException("Kurum Listesi Boş");
            }


            var listDto = mapper.Map<List<KurumMuhatapListDTO>>(getVeri);
            return listDto;


        }

        public async Task<KurumMuhatapUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await kurumRepository.GetByIdAsync(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<KurumMuhatapUpdateDTO>(getVeri);

            return dto;
        }

        public async Task UpdateAsync(KurumMuhatapUpdateDTO updateDto)
        {

            var getVeri = await kurumRepository.GetByIdAsync(updateDto.Id);

                if (getVeri == null)
                {
                    throw new Exception("Veri Yok");
                }

                mapper.Map(updateDto, getVeri);
       

                 kurumRepository.UpdateAsync(getVeri);
                await kurumRepository.SaveAsync();
            
           
        }
    }
}
