using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using EBYS.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace EBYS.Application.Services.GidenEvrakService
{
    public class GidenEvrakService(IGidenEvrakRepository evrakRepository,IMapper mapper,IImzaRotaRepository imzaRotaRepository) : IGidenEvrakService
    {

        public async Task AddAsync(GidenEvrakCreateDTO createDto)
        {
            var evrak = mapper.Map<GidenEvrak>(createDto);
            evrak.BelgeDurum = Enums.GidenEvrakDurum.Taslak;
            evrak.EvrakSayisi= 0;
            evrak.IsGelenEvrak = false;

            // 1. Akış adımlarını (İlk adımı ve rotayı) yükle
            await OlusturAkisAdimlariAsync(evrak, createDto.ImzaRotaId);

            // 2. Muhataplar ve İlgiler listesini harita metotlarına pasla
            OlusturMuhataplarVeIlgiler(evrak, createDto);

            // 3. Ekler havuzunu (Yan ekler ve Asıl Üst Yazı) tertemiz inşa et
            await OlusturEklerAsync(evrak, createDto.Ekler);    



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

        
        public async Task<List<GidenEvrakAkisListeDTO>> GidenEvraklariFiltreliListeleAsync(Enums.GidenEvrakFiltreTipi? filtreTipi)
        {
            var olusturanId = evrakRepository.GetContextUserId();

            var getVeri = await evrakRepository.FiltreliEvrakGetirAsync(olusturanId, filtreTipi);

            if (getVeri is null)
            {
                throw new EvrakBulunamadi();
            }

            return getVeri;
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
            
            var mevcutEvrak = await evrakRepository.DetayliGetirAsync(updateDto.Id);

            if (mevcutEvrak == null)
                throw new Exception("Güncellenecek evrak sistemde bulunamadı.");

            mapper.Map(updateDto, mevcutEvrak);

            GuncelleMuhataplar(mevcutEvrak, updateDto.Muhataplar);
            GuncelleIlgiler(mevcutEvrak, updateDto.Ilgiler);
            await GuncelleEklerAsync(mevcutEvrak, updateDto.Ekler);


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

        public Task<List<GidenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        private void GuncelleMuhataplar(GidenEvrak mevcutEvrak, List<GidenEvrakMuhatapSecimDTO> muhatapListesi)
        {
            var liste = muhatapListesi ?? new List<GidenEvrakMuhatapSecimDTO>();
            var dtoMuhatapIds = liste.Where(x => x.MuhatapId > 0).Select(x => x.MuhatapId).ToList();

            // Silinecekler
            var silinecekMuhataplar = mevcutEvrak.Muhataplar.Where(x => !dtoMuhatapIds.Contains(x.MuhatapId)).ToList();
            foreach (var sil in silinecekMuhataplar) mevcutEvrak.Muhataplar.Remove(sil);

            // Ekleme veya Güncelleme
            foreach (var mDto in liste)
            {
                var mevcutMuhatap = mevcutEvrak.Muhataplar.FirstOrDefault(x => x.MuhatapId == mDto.MuhatapId);
                if (mevcutMuhatap == null)
                    mevcutEvrak.Muhataplar.Add(new GidenEvrakMuhatap { MuhatapId = mDto.MuhatapId, IsBilgi = mDto.IsBilgi });
                else
                    mevcutMuhatap.IsBilgi = mDto.IsBilgi;
            }
        }

        private void GuncelleIlgiler(GidenEvrak mevcutEvrak, List<GidenEvrakIlgiUpdateDTO> ilgiListesi)
        {
            var liste = ilgiListesi ?? new List<GidenEvrakIlgiUpdateDTO>();
            var dtoIlgiIds = liste.Where(x => x.Id > 0).Select(x => x.Id).ToList();

            // Silinecekler
            var silinecekIlgiler = mevcutEvrak.İlgiler.Where(x => !dtoIlgiIds.Contains(x.Id)).ToList();
            foreach (var sil in silinecekIlgiler) mevcutEvrak.İlgiler.Remove(sil);

            // Ekleme veya Güncelleme
            foreach (var iDto in liste)
            {
                if (iDto.Id == 0)
                    mevcutEvrak.İlgiler.Add(new GidenEvrakIlgi { IlgiMetni = iDto.IlgiMetni });
                else
                {
                    var mevcutIlgi = mevcutEvrak.İlgiler.FirstOrDefault(x => x.Id == iDto.Id);
                    if (mevcutIlgi != null) mevcutIlgi.IlgiMetni = iDto.IlgiMetni;
                }
            }
        }

        private async Task GuncelleEklerAsync(GidenEvrak mevcutEvrak, List<GidenEvrakEkUpdateDTO> ekListesi)
        {
            if (mevcutEvrak.Ekler == null) mevcutEvrak.Ekler = new List<GidenEvrakEk>();
            var liste = ekListesi ?? new List<GidenEvrakEkUpdateDTO>();

            // Aynı anlamsal ayrım (Ekleme koduyla birebir simetrik 🎯)
            var incomingAsilEk = liste.FirstOrDefault(x => x.IsAsilEvrak || x.Ad == "Üst Yazı");
            var incomingYanEkler = liste.Where(x => !x.IsAsilEvrak && x.Ad != "Üst Yazı").ToList();

            // 🎯 A) SİLME AKSİYONU: Listede olmayan yan ekleri uçur, asıl evraka dokunma
            var dtoYanEkIds = incomingYanEkler.Where(x => x.Id > 0).Select(x => x.Id).ToList();
            var silinecekYanEkler = mevcutEvrak.Ekler.Where(x => !x.IsAsilEvrak && !dtoYanEkIds.Contains(x.Id)).ToList();
            foreach (var sil in silinecekYanEkler) mevcutEvrak.Ekler.Remove(sil);

            // 🎯 B) ASIL EVRAK (ÜST YAZI) GÜNCELLEME: Varsa ez, yoksa ekle
            if (incomingAsilEk != null && incomingAsilEk.Dosya != null)
            {
                var dbdekiAsilEvrak = mevcutEvrak.Ekler.FirstOrDefault(x => x.IsAsilEvrak == true);
                var fileData = await ProcessFileAsync(incomingAsilEk.Dosya);

                if (dbdekiAsilEvrak != null)
                {
                    dbdekiAsilEvrak.DosyaVerisi = fileData.Data;
                    dbdekiAsilEvrak.DosyaUzantisi = fileData.Extension;
                    dbdekiAsilEvrak.MimeType = fileData.MimeType;
                    dbdekiAsilEvrak.Ad = "Üst Yazı";
                }
                else
                {
                    mevcutEvrak.Ekler.Add(new GidenEvrakEk
                    {
                        Ad = "Üst Yazı",
                        DosyaVerisi = fileData.Data,
                        DosyaUzantisi = fileData.Extension,
                        MimeType = fileData.MimeType,
                        IsAsilEvrak = true
                    });
                }
            }

            // 🎯 C) HARİCİ YAN EKLERİ EKLE / GÜNCELLE
            foreach (var ekDto in incomingYanEkler)
            {
                if (ekDto.Id == 0 && ekDto.Dosya != null)
                {
                    var fileData = await ProcessFileAsync(ekDto.Dosya);
                    mevcutEvrak.Ekler.Add(new GidenEvrakEk
                    {
                        Ad = ekDto.Ad ?? ekDto.Dosya.FileName,
                        DosyaVerisi = fileData.Data,
                        DosyaUzantisi = fileData.Extension,
                        MimeType = fileData.MimeType,
                        IsAsilEvrak = false
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
                            mevcutEk.DosyaVerisi = fileData.Data; mevcutEk.DosyaUzantisi = fileData.Extension; mevcutEk.MimeType = fileData.MimeType;
                        }
                    }
                }
            }
        }

        private async Task OlusturAkisAdimlariAsync(GidenEvrak evrak, int imzaRotaId)
        {
            // İlk adım (Evrakı oluşturan kişi)
            evrak.AkisAdimlari.Add(new GidenEvrakAkis
            {
                KullaniciId = evrakRepository.GetContextUserId(),
                ParafMiImzaMi = Enums.ImzaTipi.Imza,
                SiraNo = 0,
                AdimDurumu = Enums.AkisAdimDurumu.Bekliyor,
                SiradakiMi = true
            });

            // Rota adımları
            var rota = await imzaRotaRepository.GetImzaRotaVeAdimlariDetay(imzaRotaId);
            if (rota?.ImzaRotaAdimlari == null || !rota.ImzaRotaAdimlari.Any())
            {
                throw new ImzaRotasıBos();
            }

            foreach (var adim in rota.ImzaRotaAdimlari.OrderBy(x => x.SiraNo))
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

        private void OlusturMuhataplarVeIlgiler(GidenEvrak evrak, GidenEvrakCreateDTO createDto)
        {
            if (createDto.Muhataplar?.Any() == true)
            {
                evrak.Muhataplar = mapper.Map<List<GidenEvrakMuhatap>>(createDto.Muhataplar);
            }

            if (createDto.Ilgiler?.Any() == true)
            {
                evrak.İlgiler = mapper.Map<List<GidenEvrakIlgi>>(createDto.Ilgiler);
            }
        }

        private async Task OlusturEklerAsync(GidenEvrak evrak, List<GidenEvrakEkCreateDTO> ekListesi)
        {
            if (evrak.Ekler == null) evrak.Ekler = new List<GidenEvrakEk>();
            var liste = ekListesi ?? new List<GidenEvrakEkCreateDTO>();

            // 🎯 A) ASIL EVRAK (ÜST YAZI) İŞLEMLERİ
            var asilEkDto = liste.FirstOrDefault(x => x.IsAsilEvrak || x.Ad == "Üst Yazı");
            if (asilEkDto?.Dosya != null)
            {
                var fileResult = await ProcessFileAsync(asilEkDto.Dosya);
                var asilEk = mapper.Map<GidenEvrakEk>(asilEkDto);

                asilEk.Ad = "Üst Yazı";
                asilEk.DosyaVerisi = fileResult.Data;
                asilEk.DosyaUzantisi = fileResult.Extension;
                asilEk.MimeType = fileResult.MimeType;
                asilEk.IsAsilEvrak = true; // Üst yazı mührü çakıldı

                // Grid simetrisi için listenin en başına koyuyoruz
                evrak.Ekler.Add(asilEk);
            }
            else
            {
                throw new Exception("Asıl evrak (Üst Yazı) içeriği boş olamaz.");
            }

            // 🎯 B) HARİCİ YAN EKLERİN DÖNGÜSÜ
            // Üst yazı haricindeki gerçek yan ekleri tertemiz dönüyoruz:
            foreach (var ekDto in liste.Where(x => !x.IsAsilEvrak && x.Ad != "Üst Yazı"))
            {
                if (ekDto.Dosya == null && string.IsNullOrEmpty(ekDto.Ad)) continue;

                var yeniEk = mapper.Map<GidenEvrakEk>(ekDto);
                yeniEk.IsAsilEvrak = false;

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

    }
}
