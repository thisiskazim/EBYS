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
            evrak.AkisAdimlari.Add(new EvrakAkis
            {
                KullaniciId = evrakRepository.GetContextUserId(),
                ParafMiImzaMi = Enums.ImzaTipi.Imza,
                SiraNo = 1,
                AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                SiradakiMi = true

            });


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
                        SiraNo = adim.SiraNo+1,
                        AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                        SiradakiMi = false

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

        public async Task DeleteAsync(int id)
        {
            var getVeri = await evrakRepository.GetByIdAsync(id);
            if (getVeri == null)
            {
                throw new Exception("Evrak bulunamadı");

            }
            evrakRepository.DeleteAsync(getVeri);
            await evrakRepository.SaveAsync();
        }

        public Task<List<GidenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GidenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await evrakRepository.DetayliGetirAsync(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<GidenEvrakUpdateDTO>(getVeri);

            return dto;
        }

        

        public async Task UpdateAsync(GidenEvrakUpdateDTO updateDto)
        {
            // 1. Evrakı tüm detaylarıyla (Muhatap, Ek, Akış vb.) çekiyoruz
            var mevcutEvrak = await evrakRepository.DetayliGetirAsync(updateDto.Id);

            if (mevcutEvrak == null)
                throw new Exception("Güncellenecek evrak sistemde bulunamadı.");

            // 2. Temel alanları DTO'dan Entity'ye aktar (Mapping)
            mapper.Map(updateDto, mevcutEvrak);

            // 3. Muhatapları Güncelle (Eskileri temizle, yenileri ekle)
            mevcutEvrak.Muhataplar.Clear();
            if (updateDto.Muhataplar != null)
            {
                foreach (var m in updateDto.Muhataplar)
                {
                    mevcutEvrak.Muhataplar.Add(new EvrakMuhatap
                    {
                        MuhatapId = m.MuhatapId,
                        IsBilgi = m.IsBilgi
                    });
                }
            }

            // 4. İlgileri Güncelle
            mevcutEvrak.İlgiler.Clear();
            if (updateDto.Ilgiler != null)
            {
                foreach (var i in updateDto.Ilgiler)
                {
                    mevcutEvrak.İlgiler.Add(new EvrakIlgi { IlgiMetni = i.IlgiMetni });
                }
            }

            // 5. Ekleri Güncelle
            mevcutEvrak.Ekler.Clear();
            if (updateDto.Ekler != null)
            {
                foreach (var e in updateDto.Ekler)
                {
                    mevcutEvrak.Ekler.Add(new EvrakEk { EkAdi = e.EkAdi });
                }
            }

            // 6. Rota/Akış Değişikliği Kontrolü
            if (mevcutEvrak.ImzaRotaId != updateDto.ImzaRotaId)
            {
                mevcutEvrak.AkisAdimlari.Clear();

                mevcutEvrak.AkisAdimlari.Add(new EvrakAkis
                {
                    KullaniciId = evrakRepository.GetContextUserId(),
                    ParafMiImzaMi = Enums.ImzaTipi.Imza,
                    SiraNo = 1,
                    AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                    SiradakiMi = true

                });



                var yeniRota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(updateDto.ImzaRotaId);

                if (yeniRota != null)
                {
                    foreach (var adim in yeniRota.ImzaRotaAdimlari.OrderBy(x => x.SiraNo))
                    {
                        mevcutEvrak.AkisAdimlari.Add(new EvrakAkis
                        {
                            KullaniciId = adim.KullaniciId,
                            ParafMiImzaMi = adim.ParafMiImzaMi,
                            SiraNo = adim.SiraNo+1,
                            AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                            SiradakiMi = false
                        });
                    }
                }
            }

            // 7. Veritabanına Yansıt
             evrakRepository.UpdateAsync(mevcutEvrak);
            await evrakRepository.SaveAsync();
        }
    }
}
