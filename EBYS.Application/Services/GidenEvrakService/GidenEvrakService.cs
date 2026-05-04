using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
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

            // Oluşturan kullanıcıyı evrak nesnesine set et
            evrak.AkisAdimlari.Add(new GidenEvrakAkis
            {
                KullaniciId = evrakRepository.GetContextUserId(),
                ParafMiImzaMi = Enums.ImzaTipi.Imza,
                SiraNo = 0,
                AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                SiradakiMi = true

            });

            // Muhatapları ekle
            if (createDto.Muhataplar != null)
            {
                foreach(var mId in createDto.Muhataplar)
                {
                    evrak.Muhataplar.Add(new GidenEvrakMuhatap
                    {
                        MuhatapId = mId.MuhatapId,
                        IsBilgi = mId.IsBilgi
                    });
                }
            }
            // Seçilen rotanın detaylarını al
            var rota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(createDto.ImzaRotaId);


            // Rota adımlarını evrak akışına ekle
            if (rota != null)
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

            // İlgileri ekle
            if (createDto.Ilgiler != null)
            {
                foreach (var i in createDto.Ilgiler)
                {
                    evrak.İlgiler.Add(new GidenEvrakIlgi { IlgiMetni = i.IlgiMetni });

                }
            }

            // Ekleri ekle
            if (createDto.Ekler != null)
            {
                foreach (var ekDto in createDto.Ekler)
                {
                    // Eğer hem isim boş hem dosya boşsa bu satırı atla (Gereksiz kayıt oluşmasın)
                    if (string.IsNullOrEmpty(ekDto.Ad) && ekDto.Dosya == null) continue;

                    var yeniEk = new GidenEvrakEk();

                    // 1. İsim Mantığı: Kullanıcı ad girdiyse onu al, girmediyse dosya adını al
                    yeniEk.Ad = !string.IsNullOrEmpty(ekDto.Ad)
                                ? ekDto.Ad
                                : ekDto.Dosya != null ? ekDto.Dosya.FileName : "Adsız Ek";

                    // 2. Dosya Mantığı: Eğer dosya varsa işle
                    if (ekDto.Dosya != null)
                    {
                        var fileResult = await ProcessFileAsync(ekDto.Dosya);
                        yeniEk.DosyaVerisi = fileResult.Data;
                        yeniEk.DosyaUzantisi = fileResult.Extension;
                        yeniEk.MimeType = fileResult.MimeType;
                    }

                    // 3. Evrak nesnesine ekle (EF otomatik olarak EvrakId'yi bağlayacak)
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
                    mevcutEvrak.Muhataplar.Add(new GidenEvrakMuhatap
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
                    mevcutEvrak.İlgiler.Add(new GidenEvrakIlgi { IlgiMetni = i.IlgiMetni });
                }
            }

            // 5. Ekleri Güncelle
            var eskiEkler = mevcutEvrak.Ekler.ToList();

            if (updateDto.Ekler != null)
            {
                foreach (var ekDto in updateDto.Ekler)
                {
                    if (ekDto.Id > 0)
                    {
                        // MEVCUT EK GÜNCELLEME
                        var mevcutEk = eskiEkler.FirstOrDefault(x => x.Id == ekDto.Id);
                        if (mevcutEk != null)
                        {
                            mevcutEk.Ad = ekDto.Ad; // İsmini her zaman güncelle

                            if (ekDto.Dosya != null)
                            {
                                // Yardımcı metodu burada kullanıyoruz
                                var fileData = await ProcessFileAsync(ekDto.Dosya);
                                mevcutEk.DosyaVerisi = fileData.Data;
                                mevcutEk.DosyaUzantisi = fileData.Extension;
                                mevcutEk.MimeType = fileData.MimeType;
                            }
                        }
                    }
                    else
                    {
                        // YENİ EK EKLEME
                        // Sadece dosya varsa ekleme yap (İsimsiz dosya olabilir ama dosyasız ek olmaz)
                        if (ekDto.Dosya != null)
                        {
                            var fileData = await ProcessFileAsync(ekDto.Dosya);
                            var yeniEk = new GidenEvrakEk
                            {
                                Ad = ekDto.Ad ?? ekDto.Dosya.FileName, // Ad boşsa dosya adını al
                                DosyaVerisi = fileData.Data,
                                DosyaUzantisi = fileData.Extension,
                                MimeType = fileData.MimeType
                            };
                            mevcutEvrak.Ekler.Add(yeniEk);
                        }
                    }
                }

                // SİLME İŞLEMİ (Aynı mantık, tertemiz)
                var gonderilenIdler = updateDto.Ekler.Where(x => x.Id > 0).Select(x => x.Id).ToList();
                var silinecekEkler = eskiEkler.Where(x => !gonderilenIdler.Contains(x.Id)).ToList();

                foreach (var silinecek in silinecekEkler)
                {
                    mevcutEvrak.Ekler.Remove(silinecek);
                }
            }

            // 2. DTO'dan gelen ekleri döngüye sokuyoruz (Create ile aynı mantık)
            //ekler güncelleme aşaması


            // 6. Rota/Akış Değişikliği Kontrolü
            if (mevcutEvrak.ImzaRotaId != updateDto.ImzaRotaId)
            {
                mevcutEvrak.AkisAdimlari.Clear();

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
