using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;


namespace EBYS.Application.Services
{
    public class ImzaRotaService(IImzaRotaRepository imzaRotaRepository, IMapper mapper) : IImzaRotaService
    {
        public async Task AddAsync(ImzaRotaCreateDTO dto)
        {
            
            RotaValidasyonalari(dto,
                 dto.RotaAdimlari.Select(x => x.KullaniciId).ToList(),
                 dto.RotaAdimlari.Count,
                 dto.RotaAdimlari.OrderBy(x => x.SiraNo).LastOrDefault()?.ParafMiImzaMi == Enums.ImzaTipi.Paraf);

            var entity = mapper.Map<ImzaRota>(dto);

            await imzaRotaRepository.AddAsync(entity);
            await imzaRotaRepository.SaveAsync();
        }
        public async Task UpdateAsync(ImzaRotaUpdateDTO dto)
        {
            var getRota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(dto.Id);

            if (getRota is null)
            {
                throw new Exception("Rota Bulunamadı");
            }

            RotaValidasyonalari(dto,
                dto.RotaAdimlari.Select(x => x.KullaniciId).ToList(),
                dto.RotaAdimlari.Count,
                dto.RotaAdimlari.OrderBy(x => x.SiraNo).LastOrDefault()?.ParafMiImzaMi == Enums.ImzaTipi.Paraf);


            var silinecekAdimlar = getRota.ImzaRotaAdimlari
                .Where(dbAdim => !dto.RotaAdimlari.Any(dtoAdim => dtoAdim.Id == dbAdim.Id))
                .ToList();

            foreach (var adim in silinecekAdimlar)
            {
                getRota.ImzaRotaAdimlari.Remove(adim);
            }


            foreach (var dtoAdim in dto.RotaAdimlari)
            {
                var dbAdim = getRota.ImzaRotaAdimlari.FirstOrDefault(x => x.Id == dtoAdim.Id && x.Id != 0);

                if (dbAdim != null)
                {
                    mapper.Map(dtoAdim, dbAdim);
                }
                else
                {

                    var yeniAdim = mapper.Map<ImzaRotaAdimi>(dtoAdim);
                    getRota.ImzaRotaAdimlari.Add(yeniAdim);
                }
            }

            imzaRotaRepository.UpdateAsync(getRota);
            await imzaRotaRepository.SaveAsync();

        }

        public async Task<List<ImzaRotaListDTO>> GetAllAsync()
        {
            var getList = await imzaRotaRepository.GetAllAsync();
            if (getList == null)
            {
                throw new InvalidOperationException("İmza Rota Listesi Boş");
            }

            return mapper.Map<List<ImzaRotaListDTO>>(getList);
        }

        public async Task DeleteAsync(int id)
        {
            var getVeri = await imzaRotaRepository.GetByIdAsync(id);
            if (getVeri == null)
            {
                throw new Exception("Rota bulunamadı");

            }
            imzaRotaRepository.DeleteAsync(getVeri);
            await imzaRotaRepository.SaveAsync();
        }



        public async Task<ImzaRotaUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<ImzaRotaUpdateDTO>(getVeri);

            return dto;

        }

       
        private void RotaValidasyonalari(ImzaRotaBaseDTO dto, List<int> kullaniciIdleri, int adimSayisi, bool sonAdimImzaMi)
        {

            if (string.IsNullOrWhiteSpace(dto.RotaAdi))
                throw new Exception("Rota adı boş olamaz.");

            if (adimSayisi == 0)
                throw new Exception("En az bir imza adımı eklemelisiniz.");

         
            if (kullaniciIdleri.Distinct().Count() != adimSayisi)
                throw new Exception("Aynı kullanıcı rotada birden fazla kez yer alamaz.");

            if (adimSayisi > 5)
                throw new Exception("En fazla 5 kişi ekleyebilirsiniz");

            if (sonAdimImzaMi)
                throw new Exception("İmza rotasında son kişi mutlaka 'İmza' tipinde olmalıdır.");

        }
    
    
    }


}
