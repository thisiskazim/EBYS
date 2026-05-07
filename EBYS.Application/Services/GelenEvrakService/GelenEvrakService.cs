using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Services.GelenEvrakService
{
    public class GelenEvrakService(IGelenEvrakRepository evrakRepository, IMapper mapper) : IGelenEvrakService
    {

      

        public async Task AddAsync(GelenEvrakCreateDTO createDto)
        {
            var evrak = mapper.Map<GelenEvrak>(createDto);
            evrak.KayitNo = await KayitNumarasiOlustur();

            // 2. Ekleri İşle (ProcessFileAsync Helper'ı kullanıyoruz)
            if (createDto.Ekler != null && createDto.Ekler.Any())
            {
                foreach (var ekDto in createDto.Ekler)
                {
                    if (ekDto.Dosya != null)
                    {
                        var fileResult = await ProcessFileAsync(ekDto.Dosya);
                        var yeniEk = mapper.Map<GelenEvrakEk>(ekDto);
                        yeniEk.DosyaVerisi = fileResult.Data;
                        yeniEk.DosyaUzantisi = fileResult.Extension;
                        yeniEk.MimeType = fileResult.MimeType;
                        evrak.Ekler.Add(yeniEk);
                    }
                }
            }


            // 3. İlgileri İşle
            if (createDto.Ilgiler != null && createDto.Ilgiler.Any())
            {
                evrak.Ilgileri = mapper.Map<List<GelenEvrakIlgi>>(createDto.Ilgiler);
            }

            // 4. İLK SEVK KAYDI (Evrakı kaydeden kişide başlar)

            var ilkSevk = new GelenEvrakSevk
            {
                GonderenKullaniciId = evrakRepository.GetContextUserId(),
                AlanKullaniciId = evrakRepository.GetContextUserId(), // İlk etapta kaydeden kişide görünür
                SevkTarihi = DateTime.Now,
                Aciklama = "Evrak Kayıt İşlemi Yapıldı."
            };
            evrak.Sevkler.Add(ilkSevk);


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

        public Task<List<GelenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GelenEvrakUpdateDTO> GetByIdAsync(int id)
        {
            var getVeri = await evrakRepository.DetayliGetirAsync(id);

            if (getVeri is null)
            {
                throw new Exception("Rota Bulunamadı");
            }
            var dto = mapper.Map<GelenEvrakUpdateDTO>(getVeri);

            return dto;
        }

        public async Task UpdateAsync(GelenEvrakUpdateDTO updateDto)
        {
            
            var mevcutEvrak = await evrakRepository.DetayliGetirAsync(updateDto.Id);

            if (mevcutEvrak == null)
            {
                throw new Exception("Güncellenecek evrak bulunamadı.");
            }

       
            mapper.Map(updateDto, mevcutEvrak);

  
            var dtoIlgiIds = updateDto.Ilgiler.Where(x => x.Id > 0).Select(x => x.Id).ToList();

        
            var silinecekIlgiler = mevcutEvrak.Ilgileri.Where(x => !dtoIlgiIds.Contains(x.Id)).ToList();
            foreach (var sil in silinecekIlgiler)
            {
                mevcutEvrak.Ilgileri.Remove(sil);
            }

 
            foreach (var ilgiDto in updateDto.Ilgiler)
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

     
            var dtoEkIds = updateDto.Ekler.Where(x => x.Id > 0).Select(x => x.Id).ToList();

     
            var silinecekEkler = mevcutEvrak.Ekler.Where(x => !dtoEkIds.Contains(x.Id)).ToList();
            foreach (var ek in silinecekEkler)
            {
                mevcutEvrak.Ekler.Remove(ek);
            }

        
            foreach (var ekDto in updateDto.Ekler)
            {
                if (ekDto.Id == 0 && ekDto.Dosya != null)
                {
                    var fileResult = await ProcessFileAsync(ekDto.Dosya);
                    var yeniEk = mapper.Map<GelenEvrakEk>(ekDto);
                    yeniEk.DosyaVerisi = fileResult.Data;
                    yeniEk.DosyaUzantisi = fileResult.Extension;
                    yeniEk.MimeType = fileResult.MimeType;
                    mevcutEvrak.Ekler.Add(yeniEk);
                }
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

    }
}
