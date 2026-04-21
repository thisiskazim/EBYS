using AutoMapper;

using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;
using Microsoft.AspNetCore.Http;

namespace EBYS.Application.Services.EvrakService
{
    public class EvrakService(IEvrakRepository evrakRepository,IMapper mapper,IImzaRotaRepository imzaRotaRepository) : IEvrakService

    {


        public async Task AddAsync(GidenEvrakCreateDTO createDto)
        {
            var evrak = mapper.Map<Evrak>(createDto);
            evrak.BelgeDurum = Enums.BelgeDurum.Taslak;
            evrak.EvrakSayisi= 1;
            evrak.IsGelenEvrak = false;

            // Oluşturan kullanıcıyı evrak nesnesine set et
            evrak.AkisAdimlari.Add(new EvrakAkis
            {
                KullaniciId = evrakRepository.GetContextUserId(),
                ParafMiImzaMi = Enums.ImzaTipi.Imza,
                SiraNo = 1,
                AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                SiradakiMi = true

            });

            // Muhatapları ekle
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
            // Seçilen rotanın detaylarını al
            var rota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(createDto.ImzaRotaId);


            // Rota adımlarını evrak akışına ekle
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

            // İlgileri ekle
            if (createDto.Ilgiler != null)
            {
                foreach (var i in createDto.Ilgiler)
                {
                    evrak.İlgiler.Add(new EvrakIlgi { IlgiMetni = i.IlgiMetni });

                }
            }

            // Ekleri ekle
            if (createDto.Ekler != null)
            {
                foreach (var ekDto in createDto.Ekler)
                {
                    // Eğer hem isim boş hem dosya boşsa bu satırı atla (Gereksiz kayıt oluşmasın)
                    if (string.IsNullOrEmpty(ekDto.Ad) && ekDto.Dosya == null) continue;

                    var yeniEk = new EvrakEk();

                    // 1. İsim Mantığı: Kullanıcı ad girdiyse onu al, girmediyse dosya adını al
                    yeniEk.Ad = !string.IsNullOrEmpty(ekDto.Ad)
                                ? ekDto.Ad
                                : (ekDto.Dosya != null ? ekDto.Dosya.FileName : "Adsız Ek");

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

            // 5. Ekleri Güncelle  // byte[] olduğu için dto ya koymadık çok veri kaplayabilir.manuel yapıyoruz
            var eskiEkler = mevcutEvrak.Ekler.ToList();

            if (updateDto.Ekler != null)
            {
                foreach (var ekDto in updateDto.Ekler)
                {
                    if (ekDto.Id > 0)
                    {
                        //  Mevcut bir eki güncelliyoruz
                        var mevcutEk = eskiEkler.FirstOrDefault(x => x.Id == ekDto.Id);
                        if (mevcutEk != null)
                        {
                            mevcutEk.Ad = ekDto.Ad; // İsmini güncelle

                            // Eğer kullanıcı yeni bir dosya yüklemişse (Dosya doluysa)
                            if (ekDto.Dosya != null)
                            {
                                using var ms = new MemoryStream();
                                await ekDto.Dosya.CopyToAsync(ms);
                                mevcutEk.DosyaVerisi = ms.ToArray();
                                mevcutEk.DosyaUzantisi = Path.GetExtension(ekDto.Dosya.FileName);
                                mevcutEk.MimeType = ekDto.Dosya.ContentType;
                            }
                            // Dosya null ise veritabanındaki eski DosyaVerisi korunur, dokunmuyoruz.
                        }
                    }
                    else
                    {
                        //  Kullanıcı güncelleme ekranında tamamen YENİ bir ek eklemiş
                        if (ekDto.Dosya != null || !string.IsNullOrEmpty(ekDto.Ad))
                        {
                            var yeniEk = new EvrakEk { Ad = ekDto.Ad };
                        
                                using var ms = new MemoryStream();
                                await ekDto.Dosya.CopyToAsync(ms);
                                yeniEk.DosyaVerisi = ms.ToArray();
                                yeniEk.DosyaUzantisi = Path.GetExtension(ekDto.Dosya.FileName);
                                yeniEk.MimeType = ekDto.Dosya.ContentType;
                            
                            mevcutEvrak.Ekler.Add(yeniEk);
                        }
                    }
                }

               
                // DTO'dan gelen listede olmayan ID'leri veritabanından siliyoruz
                var gonderilenIdler = updateDto.Ekler.Where(x => x.Id > 0).Select(x => x.Id).ToList();
                var silinecekEkler = eskiEkler.Where(x => !gonderilenIdler.Contains(x.Id)).ToList();

                foreach (var silinecek in silinecekEkler)
                {
                    mevcutEvrak.Ekler.Remove(silinecek);
                }
            }
            else
            {
              
                mevcutEvrak.Ekler.Clear();
            }

            // 2. DTO'dan gelen ekleri döngüye sokuyoruz (Create ile aynı mantık)
            //ekler güncelleme aşaması


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
