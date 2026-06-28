
using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Enum;
using EBYS.Domain.Exceptions;
using EBYS.Domain.Utilities;

namespace EBYS.Application.Services.GidenEvrakService
{
    public class GidenEvrakAkisService(IGidenEvrakRepository evrakRepository, IMapper mapper) : IGidenEvrakAkisService
    {

        public async Task<List<GidenEvrakAkisListeDTO>> ImzaBekleyenleriGetirAsync()
        {
            return await IslemBekleyenler(Enums.ImzaTipi.Imza);
        }

        public async Task<List<GidenEvrakAkisListeDTO>> ParafBekleyenleriGetirAsync()
        {
            return await IslemBekleyenler(Enums.ImzaTipi.Paraf);
        }
        public async Task<IslemSonuc> OnaylaAsync(int evrakId)
        {
     
                var entities = await evrakRepository.AkisAdimlariSorguAsync(evrakId);

                if (entities == null) throw new EvrakBulunamadi();

                var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.AdimDurumu == Enums.AkisAdimDurumu.Bekliyor);

                if(suankiAdim != null)
                {
                    suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.Onaylandi;
                    suankiAdim.SiradakiMi = false;
                    suankiAdim.creat_time = DateTime.Now;
                    suankiAdim.Not = null;
                }
                else
                    throw new Exception("Herhangi bir adım bulunamadı.");


                var sonrakiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiraNo == suankiAdim.SiraNo + 1);



                if (sonrakiAdim != null)
                {
                    sonrakiAdim.SiradakiMi = true;
                    sonrakiAdim.AdimDurumu = Enums.AkisAdimDurumu.Bekliyor;

                    if (entities.BelgeDurum == Enums.GidenEvrakDurum.Taslak || entities.BelgeDurum == Enums.GidenEvrakDurum.GeriIadeEdildi)
                    {
                        entities.BelgeDurum = Enums.GidenEvrakDurum.Imzada;
                    }
                }
                else
                {
                    entities.BelgeDurum = Enums.GidenEvrakDurum.Tamamlandi;
                    entities.EvrakSayisi++;
                    //onaylanma tarihini eklenecek
                }
                var saveResult = await evrakRepository.SaveAsync();

                if (saveResult > 0)
                    return new IslemSonuc(true, "Onay işlemi başarıyla tamamlandı.");

                return new IslemSonuc(false, "Veritabanı güncelleme sırasında bir hata oluştu.");
        }
       
        
        public async Task<IslemSonuc> IadeEtAsync(int evrakId, string not)
        {
            var entities = await evrakRepository.AkisAdimlariSorguAsync(evrakId);

            if (entities == null)
                throw new EvrakHareketiBulunamadi();

            var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.AdimDurumu == Enums.AkisAdimDurumu.Bekliyor);

            if (suankiAdim == null)
                throw new Exception("İade edilebilecek aktif bir akış adımı bulunamadı.");

            entities.BelgeDurum = Enums.GidenEvrakDurum.GeriIadeEdildi;
            suankiAdim.Not = $"İade Edildi. Gerekçe: {not}";
            

            foreach (var adim in entities.AkisAdimlari)
            {
                adim.AdimDurumu = Enums.AkisAdimDurumu.Bekliyor;
                adim.SiradakiMi = false;
            }
            suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.IadeEdildi;

            var ilkAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiraNo == 0);

            if (ilkAdim != null)
            {
                ilkAdim.SiradakiMi = true;
                ilkAdim.AdimDurumu = Enums.AkisAdimDurumu.Bekliyor;
            }

            var saveResult = await evrakRepository.SaveAsync();

            if (saveResult > 0)
                return new IslemSonuc(true,"Evrak başarıyla iade edildi ve hazırlayan memura geri gönderildi.");

            return new IslemSonuc(false, "İade işlemi sırasında veritabanı güncellenemedi.");
        }

        public async Task<IslemSonuc> ReddetAsync(int evrakId, string not)
        {
            var entities = await evrakRepository.AkisAdimlariSorguAsync(evrakId);
            if (entities == null)
                throw new EvrakHareketiBulunamadi();

            var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.AdimDurumu == Enums.AkisAdimDurumu.Bekliyor);

            if (suankiAdim == null)
                throw new Exception("Reddedilebilecek aktif bir akış adımı bulunamadı.");

            entities.BelgeDurum = Enums.GidenEvrakDurum.Reddedildi;
            suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.Reddedildi;
            suankiAdim.Not = $"Evrak Reddedildi. Gerekçe: {not}";
            suankiAdim.SiradakiMi = false;

            foreach (var adim in entities.AkisAdimlari.Where(a => a.SiraNo != suankiAdim.SiraNo))
            {
                adim.SiradakiMi = false;
            }

            var saveResult = await evrakRepository.SaveAsync();

            if (saveResult > 0)
                return new IslemSonuc(true, "Evrak başarıyla reddedildi.");

            return new IslemSonuc(false, "Reddetme işlemi sırasında veritabanı güncellenemedi.");

        }


        public async Task<List<GidenEvrakAkisListeDTO>> ImzayaGonderdigimAsync()
        {
            var userId = evrakRepository.GetContextUserId();

            var entities = await evrakRepository.ImzayaGonderdigimEvraklarAsync(userId);

            return entities.Select(e =>
            {

                var benimAdimim = e.AkisAdimlari.FirstOrDefault(a => a.KullaniciId == userId);
                var sonrakiAdim = e.AkisAdimlari.FirstOrDefault(a => a.SiraNo == benimAdimim?.SiraNo + 1);

                e.GeriCekilebilirMi = sonrakiAdim != null && sonrakiAdim.SiradakiMi;

                var suankiAdim = e.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi);
                e.SuAnKimde = suankiAdim?.KullaniciAdSoyad ?? "Tamamlandı";

                return e;


            }).ToList();
        }


        public async Task<IslemSonuc> GeriCekAsync(int evrakId)
        {
      
                var userId = evrakRepository.GetContextUserId();
                var entities = evrakRepository.AkisAdimlariSorguAsync(evrakId).Result;

                if (entities == null) return new IslemSonuc(false, "Evrak bulunamadı.");

                var benimAdimim = entities.AkisAdimlari.FirstOrDefault(a => a.KullaniciId == userId);

                if (benimAdimim == null)
                {
                    return IslemSonuc.Hata("Bu evrakta geri çekebileceğiniz bir işlem yok");
                }


                var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.SiraNo == benimAdimim.SiraNo + 1);

                if (suankiAdim == null)
                {
                    return IslemSonuc.Hata("Evrak bir sonraki kullanıcı tarafından işleme alındığı için geri çekilemez.");
                }
                else if (suankiAdim != null)
                {
                    suankiAdim.SiradakiMi = false;
                    suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.Bekliyor;

                    benimAdimim.SiradakiMi = true;
                    benimAdimim.AdimDurumu = Enums.AkisAdimDurumu.Bekliyor;
                }

                if (benimAdimim.SiraNo == 1)
                {
                    entities.BelgeDurum = Enums.GidenEvrakDurum.Taslak;
                }

                var result = await evrakRepository.SaveAsync();

                if (result > 0)
                    return IslemSonuc.İslemBasarili("Evrak başarıyla geri çekildi. 'Bekleyenler' listenize aktarıldı.");

                return IslemSonuc.Hata("İşlem sırasında bir hata oluştu.");
       
        }

        public async Task<List<GidenEvrakAkisHareketleriDTO>> EvrakHareketleriGetirAsync(int evrakId)
        {
             var entities = await evrakRepository.EvrakHareketleriGetirAsync(evrakId);

             var dtoList = mapper.Map<List<GidenEvrakAkisHareketleriDTO>>(entities);

             return dtoList;

        }


        private async Task<List<GidenEvrakAkisListeDTO>> IslemBekleyenler(Enums.ImzaTipi imzaTipi)
        {
            var userId = evrakRepository.GetContextUserId();

          
            var entities = await evrakRepository.IslemBekleyenler(userId, imzaTipi);

          
            foreach (var dto in entities)
            {
                dto.EditYapabilirMi = dto.OlusturanKullaniciId == userId;
            }

            return entities;
        }
    }
}
