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
    public class ImzaRotaService(IImzaRotaRepository imzaRotaRepository,IMapper mapper) : IImzaRotaService
    {
        public async Task AddImzaRotaAsync(ImzaRotaCreateDTO imzaRotaDto)
        {

            var get = await imzaRotaRepository.GetAllAsync();


            if (imzaRotaDto.RotaAdi==null || !imzaRotaDto.RotaAdi.Any())
                throw new Exception("Rota adı boş olamaz.");

            if (imzaRotaDto.RotaAdimlari == null || !imzaRotaDto.RotaAdimlari.Any())
                throw new Exception("En az bir imza adımı eklemelisiniz.");

            var ayniKullanici = imzaRotaDto.RotaAdimlari.Select(x => x.KullaniciId).Distinct().Count();
            if (ayniKullanici != imzaRotaDto.RotaAdimlari.Count)
                throw new Exception("Aynı kullanıcı rotada birden fazla kez yer alamaz.");

            if(imzaRotaDto.RotaAdimlari.Count > 5)
                throw new Exception("En fazla 5 kişi ekleyebilirsiniz");

            var sonAdim = imzaRotaDto.RotaAdimlari.OrderBy(x => x.SiraNo).LastOrDefault();

            if (sonAdim != null && sonAdim.ParafMiImzaMi == Enums.ImzaTipi.Paraf)    
                throw new Exception("İmza rotasında son kişi mutlaka 'İmza' tipinde olmalıdır.");
            


            var entity = mapper.Map<ImzaRota>(imzaRotaDto);

            if(entity.RotaAdi == imzaRotaDto.RotaAdi)
                throw new Exception("Aynı rota adına sahip başka bir kayıt mevcut");

            await imzaRotaRepository.AddAsync(entity);
            await imzaRotaRepository.SaveAsync();
        }
    }
}
