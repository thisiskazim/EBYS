
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
        public async Task<EvrakOnaySonuc> OnaylaAsync(int evrakId)
        {
            try
            {
                var entities = await evrakRepository.AkisAdimlariSorgu(evrakId);

                if (entities == null) throw new Exception("Evrak bulunamadı.");

                var suankiAdim = entities.AkisAdimlari.FirstOrDefault(a => a.SiradakiMi && a.AdimDurumu == Enums.AkisAdimDurumu.Bekliyor);

                suankiAdim.AdimDurumu = Enums.AkisAdimDurumu.Onaylandi;
                suankiAdim.SiradakiMi = false;

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
                    return new EvrakOnaySonuc(true, "Onay işlemi başarıyla tamamlandı.");

                return new EvrakOnaySonuc(false, "Veritabanı güncelleme sırasında bir hata oluştu.");
            }
            catch (Exception ex)
            {
                // Burada loglama yapabilirsin
                return new EvrakOnaySonuc(false, $"Beklenmedik bir hata: {ex.Message}");
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

        private async Task<List<EvrakAkisListeDTO>> IslemBekleyenler(Enums.ImzaTipi imzaTipi)
        {
            var userId = evrakRepository.GetContextUserId();

            // 1. Veriyi Repository'den Entity olarak çekiyoruz
            var entities = await evrakRepository.IslemBekleyenlenKullaniciSorgu(userId, imzaTipi);

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
