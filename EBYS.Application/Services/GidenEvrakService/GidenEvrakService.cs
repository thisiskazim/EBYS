using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace EBYS.Application.Services.GidenEvrakService
{
    public class GidenEvrakService(IGidenEvrakRepository evrakRepository,IMapper mapper,IImzaRotaRepository imzaRotaRepository) : IGidenEvrakService
    {

        public async Task AddAsync(GidenEvrakCreateDTO createDto)
        {
            var evrak = mapper.Map<GidenEvrak>(createDto);
            evrak.BelgeDurum = Enums.BelgeDurum.Taslak;
            evrak.EvrakSayisi= 0;
            evrak.IsGelenEvrak = false;

            // Oluşturan kullanıcıyı GidenEvrakAkis nesnesine set et
            evrak.AkisAdimlari.Add(new GidenEvrakAkis
            {
                KullaniciId = evrakRepository.GetContextUserId(),
                ParafMiImzaMi = Enums.ImzaTipi.Imza,
                SiraNo = 0,
                AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                SiradakiMi = true

            });

            // Muhatapları ekle
            if (createDto.Muhataplar?.Any() == true)
            {
                evrak.Muhataplar = mapper.Map<List<GidenEvrakMuhatap>>(createDto.Muhataplar);
            }

            if (createDto.Ilgiler?.Any() == true)
            {
                evrak.İlgiler = mapper.Map<List<GidenEvrakIlgi>>(createDto.Ilgiler);
            }

            // Seçilen rotanın detaylarını al
            var rota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(createDto.ImzaRotaId);

            if (rota?.ImzaRotaAdimlari != null)
            {
                foreach (var adim in rota.ImzaRotaAdimlari.OrderBy(x=>x.SiraNo))
                {
                    evrak.AkisAdimlari.Add(new GidenEvrakAkis
                    {
                        KullaniciId = adim.KullaniciId,
                        ParafMiImzaMi = adim.ParafMiImzaMi,
                        SiraNo = adim.SiraNo,
                        AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                        SiradakiMi = false

                    });
                }
            }

            if (createDto.Ekler?.Any() == true)
            {

                if (evrak.Ekler == null)
                {
                    evrak.Ekler = new List<GidenEvrakEk>();
                }
                foreach (var ekDto in createDto.Ekler)
                {
                    if (ekDto.Dosya == null && string.IsNullOrEmpty(ekDto.Ad)) continue;

                    var yeniEk = mapper.Map<GidenEvrakEk>(ekDto);

                    if (ekDto.Dosya != null)
                    {
                        var fileResult = await ProcessFileAsync(ekDto.Dosya);
                        yeniEk.DosyaVerisi = fileResult.Data;
                        yeniEk.DosyaUzantisi = fileResult.Extension;
                        yeniEk.MimeType = fileResult.MimeType;

                        if (string.IsNullOrEmpty(yeniEk.Ad))
                            yeniEk.Ad = ekDto.Dosya.FileName;
                    }
                    evrak.Ekler.Add(yeniEk);
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

        public async Task<EvrakOnizlemeBaseDTO> GidenEvrakEkOnizlemeAsync(int ekId)
        {
            try
            {
                var getVeri = await evrakRepository.GidenEvrakEkDosyaByIdAsync(ekId);

                if (getVeri == null)
                {
                    throw new Exception("Dosya bulunamadı");
                }
                var dto = mapper.Map<EvrakOnizlemeBaseDTO>(getVeri);
                return dto;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Veritabanı Hatası: " + message);
            }

        }

        public async Task UpdateAsync(GidenEvrakUpdateDTO updateDto)
        {
            // 1. Evrakı tüm detaylarıyla (Muhatap, Ek, Akış vb.) çekiyoruz
            var mevcutEvrak = await evrakRepository.DetayliGetirAsync(updateDto.Id);

            if (mevcutEvrak == null)
                throw new Exception("Güncellenecek evrak sistemde bulunamadı.");

            // 2. Temel alanları DTO'dan Entity'ye aktar (Mapping)
            mapper.Map(updateDto, mevcutEvrak);


           
            var muhatapListesi = updateDto.Muhataplar ?? new List<GidenEvrakMuhatapSecimDTO>();
            var dtoMuhatapIds = muhatapListesi.Where(x => x.MuhatapId > 0).Select(x => x.MuhatapId).ToList();
            var silinecekMuhataplar = mevcutEvrak.Muhataplar.Where(x => !dtoMuhatapIds.Contains(x.MuhatapId)).ToList();

            foreach (var sil in silinecekMuhataplar) mevcutEvrak.Muhataplar.Remove(sil);

            foreach (var mDto in muhatapListesi)
            {
                var mevcutMuhatap = mevcutEvrak.Muhataplar.FirstOrDefault(x => x.MuhatapId == mDto.MuhatapId);
                if (mevcutMuhatap == null)
                {
                    mevcutEvrak.Muhataplar.Add(new GidenEvrakMuhatap { MuhatapId = mDto.MuhatapId, IsBilgi = mDto.IsBilgi });
                }
                else
                {
                    mevcutMuhatap.IsBilgi = mDto.IsBilgi;
                }
            }



            var ilgiListesi = updateDto.Ilgiler ?? new List<GidenEvrakIlgiUpdateDTO>();
            var dtoIlgiIds = ilgiListesi.Where(x => x.Id > 0).Select(x => x.Id).ToList();

            var silinecekIlgiler = mevcutEvrak.İlgiler.Where(x => !dtoIlgiIds.Contains(x.Id)).ToList();
            foreach (var sil in silinecekIlgiler) mevcutEvrak.İlgiler.Remove(sil);

            foreach (var iDto in ilgiListesi)
            {
                if (iDto.Id == 0)
                {
                    mevcutEvrak.İlgiler.Add(new GidenEvrakIlgi { IlgiMetni = iDto.IlgiMetni });
                }
                else
                {
                    var mevcutIlgi = mevcutEvrak.İlgiler.FirstOrDefault(x => x.Id == iDto.Id);
                    if (mevcutIlgi != null) mevcutIlgi.IlgiMetni = iDto.IlgiMetni;
                }
            }



            var ekListesi = updateDto.Ekler ?? new List<GidenEvrakEkUpdateDTO>();
            var dtoEkIds = ekListesi.Where(x => x.Id > 0).Select(x => x.Id).ToList();

            // Silinecek Ekler
            var silinecekEkler = mevcutEvrak.Ekler.Where(x => !dtoEkIds.Contains(x.Id)).ToList();
            foreach (var sil in silinecekEkler) mevcutEvrak.Ekler.Remove(sil);

            // Ekleme/Güncelleme
            foreach (var ekDto in ekListesi)
            {
                if (ekDto.Id == 0 && ekDto.Dosya != null)
                {
                    var fileData = await ProcessFileAsync(ekDto.Dosya);
                    mevcutEvrak.Ekler.Add(new GidenEvrakEk
                    {
                        Ad = ekDto.Ad ?? ekDto.Dosya.FileName,
                        DosyaVerisi = fileData.Data,
                        DosyaUzantisi = fileData.Extension,
                        MimeType = fileData.MimeType
                    });
                }
                else if (ekDto.Id > 0)
                {
                    var mevcutEk = mevcutEvrak.Ekler.FirstOrDefault(x => x.Id == ekDto.Id);
                    if (mevcutEk != null)
                    {
                        mevcutEk.Ad = ekDto.Ad;
                        if (ekDto.Dosya != null)
                        {
                            var fileData = await ProcessFileAsync(ekDto.Dosya);
                            mevcutEk.DosyaVerisi = fileData.Data;
                            mevcutEk.DosyaUzantisi = fileData.Extension;
                            mevcutEk.MimeType = fileData.MimeType;
                        }
                    }
                }
            }

            // 2. DTO'dan gelen ekleri döngüye sokuyoruz (Create ile aynı mantık)
            //ekler güncelleme aşaması


            // 6. Rota/Akış Değişikliği Kontrolü
            if (mevcutEvrak.ImzaRotaId != updateDto.ImzaRotaId)
            {
                mevcutEvrak.AkisAdimlari.Clear(); // Rota değişirse akış sıfırlanır, bu doğru.

                // Kendisini 1. Adıma Ekle
                mevcutEvrak.AkisAdimlari.Add(new GidenEvrakAkis
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
                        mevcutEvrak.AkisAdimlari.Add(new GidenEvrakAkis
                        {
                            KullaniciId = adim.KullaniciId,
                            ParafMiImzaMi = adim.ParafMiImzaMi,
                            SiraNo = adim.SiraNo + 1,
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
        private async Task<(byte[] Data, string Extension, string MimeType)> ProcessFileAsync(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            return (
                Data: memoryStream.ToArray(),
                Extension: Path.GetExtension(file.FileName),
                MimeType: file.ContentType
            );
        }
    }
}
