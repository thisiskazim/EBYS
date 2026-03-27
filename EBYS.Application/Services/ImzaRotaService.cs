using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Services
{
    public class ImzaRotaService(IImzaRotaRepository imzaRotaRepository, IMapper mapper) : IImzaRotaService
    {
        public async Task AddImzaRotaAsync(ImzaRotaCreateDTO dto)
        {
            
            RotaValidasyonalari(dto,
                 dto.RotaAdimlari.Select(x => x.KullaniciId).ToList(),
                 dto.RotaAdimlari.Count,
                 dto.RotaAdimlari.OrderBy(x => x.SiraNo).LastOrDefault()?.ParafMiImzaMi == Enums.ImzaTipi.Imza);

            var entity = mapper.Map<ImzaRota>(dto);

            await imzaRotaRepository.AddAsync(entity);
            await imzaRotaRepository.SaveAsync();
        }


        public async Task UpdateImzaRotaAsync(ImzaRotaUpdateDTO dto)
        {
            var getRota = await imzaRotaRepository.GetByIdAsync(dto.Id, x => x.ImzaRotaAdimlari);

            if (getRota is null)
            {
                throw new Exception("Rota Bulunamadı");
            }

            RotaValidasyonalari(dto,
                 dto.RotaAdimlari.Select(x => x.KullaniciId).ToList(),
                 dto.RotaAdimlari.Count,
                 dto.RotaAdimlari.OrderBy(x => x.SiraNo).LastOrDefault()?.ParafMiImzaMi == Enums.ImzaTipi.Imza);

            mapper.Map(dto, getRota);

        
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

            // 5. Kaydet
            await imzaRotaRepository.UpdateAsync();
            await imzaRotaRepository.SaveAsync();

        }

        public async Task<List<ImzaRotaAdimlariBaseDTO>> GetAllImzaRotasAsync()
        {
            var entities = await imzaRotaRepository.GetAllAsync();
            return mapper.Map<List<ImzaRotaAdimlariBaseDTO>>(entities);
        }
   
    
        public void RotaValidasyonalari(ImzaRotaBaseDTO dto, List<int> kullaniciIdleri, int adimSayisi, bool sonAdimImzaMi)
        {

            if (string.IsNullOrWhiteSpace(dto.RotaAdi))
                throw new Exception("Rota adı boş olamaz.");

            if (adimSayisi == 0)
                throw new Exception("En az bir imza adımı eklemelisiniz.");

         
            if (kullaniciIdleri.Distinct().Count() != adimSayisi)
                throw new Exception("Aynı kullanıcı rotada birden fazla kez yer alamaz.");

            if (adimSayisi > 5)
                throw new Exception("En fazla 5 kişi ekleyebilirsiniz");

            if (!sonAdimImzaMi)
                throw new Exception("İmza rotasında son kişi mutlaka 'İmza' tipinde olmalıdır.");

        }
    
    
    }


}
