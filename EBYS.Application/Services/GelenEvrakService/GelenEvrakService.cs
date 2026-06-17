using AutoMapper;
using EBYS.Application.DTOs;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.IService.IGelenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Enum;
using EBYS.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Runtime.InteropServices;



namespace EBYS.Application.Services.GelenEvrakService
{
    public class GelenEvrakService(IGelenEvrakRepository evrakRepository, IMapper mapper) : IGelenEvrakService
    {

        public async Task AddAsync(GelenEvrakCreateDTO createDto)
        {
                var evrak = mapper.Map<GelenEvrak>(createDto);
                evrak.KayitNo = await KayitNumarasiOlustur();

          
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
                       
                        if (ekDto.Dosya == null && string.IsNullOrEmpty(ekDto.Ad)) continue;

                        var yeniEk = mapper.Map<GelenEvrakEk>(ekDto);

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


                if (evrak.Sevkler == null)
                {
                    evrak.Sevkler = new List<GelenEvrakSevk>();
                }

                var ilkSevk = new GelenEvrakSevk
                {
                    SevkEdenKullaniciId = evrakRepository.GetContextUserId(),
                    SevkTarihi = DateTime.Now,
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

        public async Task<List<GelenEvrakListDTO>> GelenEvraklariFiltreliListeleAsync(Enums.GelenEvrakDurumu? durum)
        {
                var olusturanId = evrakRepository.GetContextUserId();

                var getVeri = await evrakRepository.FiltreliEvrakGetirAsync(olusturanId,durum);

                if (getVeri is null)
                {
                    throw new Exception("Gelen Evrak bulunamadı");
                }

                foreach (var veri in getVeri)
                {
                    veri.EditYapabilirMi = veri.OlusturanId == olusturanId;
                    veri.IslemSirasiBendeMi = veri.AlanKullaniciId == olusturanId;


                }
               return getVeri;

        }

        public async Task<GelenEvrakUpdateDTO> GetByIdAsync(int id)
        {
          
                var getVeri = await evrakRepository.DetayliGetirByIdAsync(id);

                if (getVeri is null)
                {
                throw new RotaBulunamadi();
                }
                var dto = mapper.Map<GelenEvrakUpdateDTO>(getVeri);

                return dto;
        }

        public async Task UpdateAsync(GelenEvrakUpdateDTO updateDto)
        {
           
                var mevcutEvrak = await evrakRepository.DetayliGetirByIdAsync(updateDto.Id);

                if (mevcutEvrak == null)
                {
                    throw new EvrakHareketiBulunamadi();
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

        public async Task<EvrakOnizlemeBaseDTO> GelenEvrakEkOnizlemeAsync(int ekId)
        {
     
                var getVeri = await evrakRepository.GelenEvrakEkDosyaByIdAsync(ekId);

                if (getVeri == null)
                {
                    throw new DosyaBulunamadi();    
            }
                var dto = mapper.Map<EvrakOnizlemeBaseDTO>(getVeri);
                return dto;
        }

        public async Task<List<GelenEvrakSevkListDTO>> GelenEvrakHareketleri(int evrakId)
        {
                var getVeri = await evrakRepository.GelenEvrakSevkHareketleriAsync(evrakId);

                if (getVeri is null)
                {
                    throw new EvrakBulunamadi();
            }

                return getVeri;
        }

        public async Task<GelenEvrakSevk> SahsimaTeslimAl(int evrakId)
        {
            var currentUserId = evrakRepository.GetContextUserId();
            var sevkEntity = await evrakRepository.SevkGetirByIdAsync(evrakId);

            if(sevkEntity is null)
            {
                throw new BaskaBirKullaniciTarafindanTeslimAlinmis();  
            }

            sevkEntity.AlanKullaniciId = currentUserId;
            sevkEntity.SevkTarihi = DateTime.Now; 
            sevkEntity.GelenEvrakDurumEnum = Enums.GelenEvrakDurumu.TeslimAlindi;

            await evrakRepository.SaveAsync();

            return sevkEntity;

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
        public Task<List<GelenEvrakListDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
