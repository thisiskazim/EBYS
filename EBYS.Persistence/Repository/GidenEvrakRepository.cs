using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using EBYS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EBYS.Domain.Enum.Enums;

namespace EBYS.Persistence.Repository
{
    public class GidenEvrakRepository : GenericRepository<GidenEvrak>, IGidenEvrakRepository
    {

        private readonly IMapper _mapper;
        public GidenEvrakRepository(EBYSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<GidenEvrak> AkisAdimlariSorguAsync(int evrakId)
        {

            return await _context.Evraklar
                      .Include(x => x.AkisAdimlari)
                      .ThenInclude(a => a.Kullanici)
                      .FirstOrDefaultAsync(e => e.Id == evrakId); ;
        }

        public async Task<GidenEvrak> DetayliGetirAsync(int id)
        {
            return await _context.Evraklar
                 .Include(x => x.Muhataplar)
                    .ThenInclude(m => m.Muhatap)
                 .Include(x => x.İlgiler)
                 .Include(x => x.Ekler)
                 .Include(x => x.AkisAdimlari)
                 .Include(x => x.EvrakKonuKodu)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<GidenEvrakAkis>> EvrakHareketleriGetirAsync(int evrakId)
        {
            return await _context.EvrakAkislari
                    .Include(x => x.Kullanici)
                    .Where(x => x.EvrakId == evrakId)
                    .OrderBy(x => x.Id)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<GidenEvrakAkisListeDTO>> ImzayaGonderdigimEvraklarAsync(int userId)
        {
            return await _context.Evraklar
        .Where(e => e.BelgeDurum == Enums.GidenEvrakDurum.Imzada &&
                    e.AkisAdimlari.Any(a => a.KullaniciId == userId && a.AdimDurumu == Enums.AkisAdimDurumu.Onaylandi))
        .OrderByDescending(e => e.creat_time)
        .AsNoTracking()
        .ProjectTo<GidenEvrakAkisListeDTO>(_mapper.ConfigurationProvider)
        .ToListAsync();
        }


        public async Task<GidenEvrakEk> GidenEvrakEkDosyaByIdAsync(int ekId)
        {
            return await _context.EvrakEkler
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == ekId);
        }

        public async Task<List<GidenEvrakAkisListeDTO>> IslemBekleyenler(int userId, Enums.ImzaTipi imzaTipi)
        {
            return await _context.Evraklar.AsNoTracking().Where(e => (e.BelgeDurum == Enums.GidenEvrakDurum.Taslak || e.BelgeDurum == Enums.GidenEvrakDurum.Imzada || e.BelgeDurum== Enums.GidenEvrakDurum.GeriIadeEdildi) && !e.isDelete &&
                           (e.AkisAdimlari.Any(a => a.KullaniciId == userId && a.SiradakiMi && a.ParafMiImzaMi == imzaTipi)))
                            .AsNoTracking()
                            .ProjectTo<GidenEvrakAkisListeDTO>(_mapper.ConfigurationProvider)
                            .ToListAsync();
        }



        public async Task<List<GidenEvrakAkisListeDTO>> FiltreliEvrakGetirAsync(int? currentUserId, GidenEvrakFiltreTipi? filtreTipi)
        {
            var query = _context.Evraklar.AsNoTracking().Where(x => !x.isDelete && x.IsGelenEvrak == false);

            if (filtreTipi.HasValue)
            {
                switch (filtreTipi.Value)
                {

                    case GidenEvrakFiltreTipi.TumGidenEvraklar:
                        query = query.Where(x => x.EvrakSayisi > 0 && x.BelgeDurum == Enums.GidenEvrakDurum.Tamamlandi);
                        break;

                    case GidenEvrakFiltreTipi.IadeEttiklerim:
                        query = query.Where(x => x.AkisAdimlari.Any(a => a.KullaniciId == currentUserId && a.AdimDurumu == Enums.AkisAdimDurumu.IadeEdildi));
                        break;

                    case GidenEvrakFiltreTipi.SahsimaIadeEdilenler:
                        query = query.Where(x => x.OlusturanId == currentUserId && x.BelgeDurum == Enums.GidenEvrakDurum.GeriIadeEdildi);
                        break;

                    case GidenEvrakFiltreTipi.Reddettiklerim:
                        query = query.Where(x => x.AkisAdimlari.Any(a => a.KullaniciId == currentUserId && a.AdimDurumu == Enums.AkisAdimDurumu.Reddedildi));
                        break;

                    case GidenEvrakFiltreTipi.BanaRedDonen:
                        query = query.Where(x => x.OlusturanId == currentUserId && x.BelgeDurum == Enums.GidenEvrakDurum.Reddedildi && x.AkisAdimlari.Any(a => a.AdimDurumu == Enums.AkisAdimDurumu.Reddedildi));
                        break;

                }
            }
            return await query
            .OrderByDescending(x => x.creat_time)
            .ProjectTo<GidenEvrakAkisListeDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

       
    }
}
