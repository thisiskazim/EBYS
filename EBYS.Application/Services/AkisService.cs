
using AutoMapper;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.Interfaces.IService;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Enum;
using EBYS.Domain.Utilities;

namespace EBYS.Application.Services
{
    public class AkisService(IEvrakRepository evrakRepository, IMapper mapper) : IAkisService
    {

        public async Task<List<EvrakAkisListeDTO>> ImzaBekleyenleriGetirAsync()
        {
            return await IslemBekleyenler(Enums.ImzaTipi.Imza);
        }

        public Task<List<EvrakAkisListeDTO>> ParafBekleyenleriGetirAsync()
        {
            return IslemBekleyenler(Enums.ImzaTipi.Paraf);
        }
        public async Task<IslemSonuc> OnaylaAsync(int evrakId)
        {
            try
            {
                var entities = await evrakRepository.AkisAdimlariSorguAsync(evrakId);

                if (entities == null) throw new Exception("Evrak bulunamadı.");

                var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.AdimDurumu == Enums.AkisAdimDurumu.Bekliyor);

                suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.Onaylandi;
                suankiAdim.SiradakiMi = false;
                suankiAdim.creat_time = DateTime.Now;

                var sonrakiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiraNo == suankiAdim.SiraNo + 1);



                if (sonrakiAdim != null)
                {
                    sonrakiAdim.SiradakiMi = true;
                    if (entities.BelgeDurum == Enums.BelgeDurum.Taslak)
                    {
                        entities.BelgeDurum = Enums.BelgeDurum.Imzada;
                    }

                }
                else
                {
                    entities.BelgeDurum = Enums.BelgeDurum.Tamamlandi;

                    //entities.EvrakSayisi= entities.EvrakSayisi + 1;
                    //onaylanma tarihini eklenecek
                }
                var saveResult = await evrakRepository.SaveAsync();

                if (saveResult > 0)
                    return new IslemSonuc(true, "Onay işlemi başarıyla tamamlandı.");

                return new IslemSonuc(false, "Veritabanı güncelleme sırasında bir hata oluştu.");
            }
            catch (Exception ex)
            {
                // Burada loglama yapabilirsin
                return new IslemSonuc(false, $"Beklenmedik bir hata: {ex.Message}");
            }
        }
        public Task<bool> IadeEtAsync(int evrakId, string neden)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReddetAsync(int evrakId, string neden)
        {
            throw new NotImplementedException();
        }


        public async Task<List<EvrakAkisListeDTO>> ImzayaGonderdigimAsync()
        {
            var userId = evrakRepository.GetContextUserId();

            var entities = await evrakRepository.ImzayaGonderdigimEvraklarAsync(userId);

            return entities.Select(e =>
            {
                var dto = mapper.Map<EvrakAkisListeDTO>(e);

                var benimAdimim = e.AkisAdimlari.FirstOrDefault(a => a.KullaniciId == userId);
                var sonrakiAdim = e.AkisAdimlari.FirstOrDefault(a => a.SiraNo == benimAdimim?.SiraNo + 1);

                dto.GeriCekilebilirMi = sonrakiAdim != null && sonrakiAdim.SiradakiMi;

                var suankiAdim = e.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi);
                dto.SuAnKimde = suankiAdim?.Kullanici.AdSoyad ?? "Tamamlandı";




                return dto;


            }).ToList();
        }


        public async Task<IslemSonuc> GeriCekAsync(int evrakId)
        {
            try
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
                    entities.BelgeDurum = Enums.BelgeDurum.Taslak;
                }

                var result = await evrakRepository.SaveAsync();

                if (result > 0)
                    return IslemSonuc.İslemBasarili("Evrak başarıyla geri çekildi. 'Bekleyenler' listenize aktarıldı.");

                return IslemSonuc.Hata("İşlem sırasında bir hata oluştu.");
            }
            catch (Exception ex)
            {
                return IslemSonuc.İslemBasarili($"Beklenmedik hata: {ex.Message}");
            }
        }

        public async Task<List<EvrakAkisHareketleriDTO>> EvrakHareketleriGetirAsync(int evrakId)
        {
             var entities = await evrakRepository.EvrakHareketleriGetirAsync(evrakId);

             var dtoList = mapper.Map<List<EvrakAkisHareketleriDTO>>(entities);

             return dtoList;

        }


        private async Task<List<EvrakAkisListeDTO>> IslemBekleyenler(Enums.ImzaTipi imzaTipi)
        {
            var userId = evrakRepository.GetContextUserId();

            // 1. Veriyi Repository'den Entity olarak çekiyoruz
            var entities = await evrakRepository.IslemBekleyenlenKullaniciSorguAsync(userId, imzaTipi);

            // 2. AutoMapper ile Entity -> DTO dönüşümü (MAPPING)
            var dtoList = mapper.Map<List<EvrakAkisListeDTO>>(entities);

            // 3. Özel iş kuralını (CanEdit) döngüyle veya mapping sırasında set edebiliriz
            foreach (var dto in dtoList)
            {
                dto.EditYapabilirMi = dto.OlusturanKullaniciId == userId;
            }

            return dtoList;
        }
    }
}
