using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;

namespace EBYS.Application.Services.EvrakService
{
    public class EvrakService(IEvrakRepository evrakRepository,IMapper mapper,IImzaRotaRepository imzaRotaRepository) : IEvrakService

    {
        public async Task AddAsync(GidenEvrakCreateDTO createDto)
        {
            var evrak = mapper.Map<Evrak>(createDto);
            evrak.BelgeDurum = Enums.BelgeDurum.Taslak;
            evrak.EvrakSayisi= "E.-1";
            evrak.IsGelenEvrak = false;


            if (createDto.Muhataplar != null)
            {
                foreach(var mId in createDto.Muhataplar)
                {
                    evrak.Muhataplar.Add(new EvrakMuhatap
                    {
                        MuhatapId = mId.MuhatapId,
                        IsBilgi = mId.IsBilgi
                    });
                }
            }

            var rota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(createDto.ImzaRotaId);

            if (rota != null)
            {
                foreach (var adim in rota.ImzaRotaAdimlari.OrderBy(x=>x.SiraNo))
                {
                    evrak.AkisAdimlari.Add(new EvrakAkis
                    {
                        KullaniciId = adim.KullaniciId,
                        ParafMiImzaMi = adim.ParafMiImzaMi,
                        SiraNo = adim.SiraNo,
                        AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                        SiradakiMi = (adim.SiraNo == 1)

                    });
                }
            }

            if (createDto.Ilgiler != null)
            {
                foreach (var i in createDto.Ilgiler)
                {
                    evrak.İlgiler.Add(new EvrakIlgi { IlgiMetni = i.IlgiMetni });

                }
            }

            if (createDto.Ekler != null)
            {
                foreach (var i in createDto.Ekler)
                {
                    evrak.Ekler.Add(new EvrakEk { EkAdi = i.EkAdi });

                }
            }

            await evrakRepository.AddAsync(evrak);
            await evrakRepository.SaveAsync();

        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<GidenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<GidenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(GidenEvrakUpdateDTO updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
