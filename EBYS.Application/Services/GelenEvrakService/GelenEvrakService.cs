using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GelenEvrak;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;



namespace EBYS.Application.Services.GelenEvrakService
{
    public class GelenEvrakService(IGelenEvrakRepository evrakRepository, IMapper mapper) : IGelenEvrakService
    {

        public async Task AddAsync(GelenEvrakCreateDTO createDto)
        {

            try
            {
                var evrak = mapper.Map<GelenEvrak>(createDto);
                evrak.KayitNo = await KayitNumarasiOlustur();

                // 3. İlgileri İşle
                if (createDto.Ilgiler != null && createDto.Ilgiler.Any())
                {
                    evrak.Ilgileri = mapper.Map<List<GelenEvrakIlgi>>(createDto.Ilgiler);
                }


                if (createDto.Ekler?.Any() == true)
                {

                    if (evrak.Ekler == null)
                    {
                        evrak.Ekler = new List<GelenEvrakEk>();
                    }
                    foreach (var ekDto in createDto.Ekler)
                    {
                        // Eğer dosya yoksa ve isim de girilmemişse boş kayıt atmayalım
                        if (ekDto.Dosya == null && string.IsNullOrEmpty(ekDto.Ad)) continue;

                        var yeniEk = mapper.Map<GelenEvrakEk>(ekDto);

                        if (ekDto.Dosya != null)
                        {
                            var fileResult = await ProcessFileAsync(ekDto.Dosya);
                            yeniEk.DosyaVerisi = fileResult.Data;
                            yeniEk.DosyaUzantisi = fileResult.Extension;
                            yeniEk.MimeType = fileResult.MimeType;

                            // İsim boşsa dosya adını varsayılan yap
                            if (string.IsNullOrEmpty(yeniEk.Ad))
                                yeniEk.Ad = ekDto.Dosya.FileName;
                        }
                        evrak.Ekler.Add(yeniEk);
                    }
                }

                // 4. İLK SEVK KAYDI (Evrakı kaydeden kişide başlar)

                if (evrak.Sevkler == null)
                {
                    evrak.Sevkler = new List<GelenEvrakSevk>();
                }

                var ilkSevk = new GelenEvrakSevk
                {
                    SevkEdenKullaniciId = evrakRepository.GetContextUserId(),
                    SevkTarihi = DateTime.Now,
                    Aciklama = "Evrak Kayıt İşlemi Yapıldı."
                };
                evrak.Sevkler.Add(ilkSevk);


                await evrakRepository.AddAsync(evrak);
                await evrakRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                // Debug modundayken ex.InnerException.Message kısmına bak abi
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Veritabanı Hatası: " + message);
            }
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

        public async Task<List<GelenEvrakListDTO>> GetAllAsync()
        {
            try
            {
                var olusturanId = evrakRepository.GetContextUserId();

                var getVeri = await evrakRepository.GelenEvrakListAsync();

                if (getVeri is null)
                {
                    throw new Exception("Gelen Evrak bulunamadı");
                }

                foreach (var veri in getVeri)
                {
                    veri.EditYapabilirMi = veri.OlusturanId == olusturanId;

                }


               return getVeri;


            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Veritabanı Hatası: " + message);
            }

        }

        public async Task<GelenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            try
            {
                var getVeri = await evrakRepository.DetayliGetirAsync(id);

                if (getVeri is null)
                {
                    throw new Exception("Rota Bulunamadı");
                }
                var dto = mapper.Map<GelenEvrakUpdateDTO>(getVeri);

                return dto;
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Veritabanı Hatası: " + message);
            }

        }

        public async Task UpdateAsync(GelenEvrakUpdateDTO updateDto)
        {
            try
            {
                var mevcutEvrak = await evrakRepository.DetayliGetirAsync(updateDto.Id);

                if (mevcutEvrak == null)
                {
                    throw new Exception("Güncellenecek evrak bulunamadı.");
                }

                mapper.Map(updateDto, mevcutEvrak);


                var ilgiListesi = updateDto.Ilgiler ?? new List<GelenEvrakIlgiUpdateDTO>();
                var dtoIlgiIds = ilgiListesi.Where(x => x.Id > 0).Select(x => x.Id).ToList();

                var silinecekIlgiler = mevcutEvrak.Ilgileri.Where(x => !dtoIlgiIds.Contains(x.Id)).ToList();
                foreach (var sil in silinecekIlgiler)
                {
                    mevcutEvrak.Ilgileri.Remove(sil);
                }

                foreach (var ilgiDto in ilgiListesi)
                {
                    if (ilgiDto.Id == 0)
                    {
                        mevcutEvrak.Ilgileri.Add(mapper.Map<GelenEvrakIlgi>(ilgiDto));
                    }
                    else
                    {
                        var mevcutIlgi = mevcutEvrak.Ilgileri.FirstOrDefault(x => x.Id == ilgiDto.Id);
                        if (mevcutIlgi != null)
                        {
                            mapper.Map(ilgiDto, mevcutIlgi);
                        }
                    }
                }


                var ekListesi = updateDto.Ekler ?? new List<GelenEvrakEkUpdateDTO>();
                var dtoEkIds = ekListesi.Where(x => x.Id > 0).Select(x => x.Id).ToList();

                var silinecekEkler = mevcutEvrak.Ekler.Where(x => !dtoEkIds.Contains(x.Id)).ToList();
                foreach (var ek in silinecekEkler)
                {
                    mevcutEvrak.Ekler.Remove(ek);
                }

                foreach (var ekDto in ekListesi)
                {
                    if (ekDto.Id == 0 && ekDto.Dosya != null)
                    {
                        var fileResult = await ProcessFileAsync(ekDto.Dosya);
                        var yeniEk = mapper.Map<GelenEvrakEk>(ekDto);
                        yeniEk.DosyaVerisi = fileResult.Data;
                        yeniEk.DosyaUzantisi = fileResult.Extension;
                        yeniEk.MimeType = fileResult.MimeType;

                        if (string.IsNullOrEmpty(yeniEk.Ad))
                            yeniEk.Ad = ekDto.Dosya.FileName;

                        mevcutEvrak.Ekler.Add(yeniEk);
                    }
                    else if (ekDto.Id > 0)
                    {
                        var mevcutEk = mevcutEvrak.Ekler.FirstOrDefault(x => x.Id == ekDto.Id);
                        if (mevcutEk != null)
                        {
                            mevcutEk.Ad = ekDto.Ad;

                            if (ekDto.Dosya != null)
                            {
                                var fileResult = await ProcessFileAsync(ekDto.Dosya);
                                mevcutEk.DosyaVerisi = fileResult.Data;
                                mevcutEk.DosyaUzantisi = fileResult.Extension;
                                mevcutEk.MimeType = fileResult.MimeType;
                            }
                        }
                    }
                }

                evrakRepository.UpdateAsync(mevcutEvrak);
                await evrakRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                throw new Exception("Veritabanı Hatası: " + message);
            }

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
        private async Task<string> KayitNumarasiOlustur()
        {
            int yil = DateTime.Now.Year;

            int count = await evrakRepository.KayitNumarasiOlustur(yil);

            return $"{yil}/{count + 1}";
        }

        public async Task<EvrakOnizlemeBaseDTO> GelenEvrakEkOnizlemeAsync(int ekId)
        {
            try
            {
                var getVeri = await evrakRepository.GelenEvrakEkDosyaByIdAsync(ekId);

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
    }
}
