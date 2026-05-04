using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GidenEvrak;
using EBYS.Domain.Enum;
using EBYS.Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence.Repository
{
    public class GidenEvrakRepository : GenericRepository<GidenEvrak>, IGidenEvrakRepository
    {
        public GidenEvrakRepository(EBYSContext context) : base(context) { }

        public async Task<GidenEvrak> AkisAdimlariSorguAsync(int evrakId)
        {

            return await _context.Evraklar
                      .Include(x => x.AkisAdimlari)
                      .ThenInclude(a => a.Kullanici) // Burası kritik!
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
                    .OrderBy(x => x.SiraNo)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<GidenEvrak>> ImzayaGonderdigimEvraklarAsync(int userId)
        {
            return await _context.Evraklar
                .Include(x => x.EvrakKonuKodu)
                .Include(x => x.Olusturan)
                .Include(x => x.AkisAdimlari)
                        .ThenInclude(a => a.Kullanici)//şu an kimde
                .Where(e => e.BelgeDurum == Enums.BelgeDurum.Imzada && e.AkisAdimlari.Any(a => a.KullaniciId == userId && a.AdimDurumu == Enums.AkisAdimDurumu.Onaylandi))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<GidenEvrak>> IslemBekleyenlenKullaniciSorguAsync(int userId, Enums.ImzaTipi imzaTipi)//evrak mı parafmı berkleyenlerde kullanıcıya göre sorgu
        {

            return await _context.Evraklar
                .Include(x => x.EvrakKonuKodu)
                .Include(x => x.Olusturan)
                .Include(x => x.AkisAdimlari)
                .Where(e => (e.BelgeDurum == Enums.BelgeDurum.Taslak || e.BelgeDurum == Enums.BelgeDurum.Imzada) &&
                            (e.AkisAdimlari.Any(a => a.KullaniciId == userId && a.SiradakiMi && a.ParafMiImzaMi == imzaTipi)))
                .AsNoTracking()
                .ToListAsync();
        }


    }
}
