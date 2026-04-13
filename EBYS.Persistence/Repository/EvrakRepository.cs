using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using EBYS.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence.Repository   
{
    public class EvrakRepository : GenericRepository<Evrak>, IEvrakRepository
    {
        public EvrakRepository(EBYSContext context) : base(context){ }

        public async Task<Evrak> DetayliGetirAsync(int id)
        {
            return await _context.Evraklar
         .Include(x => x.Muhataplar)
         .Include(x => x.İlgiler)
         .Include(x => x.Ekler)
         .Include(x => x.AkisAdimlari)
         .Include(x => x.EvrakKonuKodu)
         .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Evrak>> ImzaBekleyenlenKullaniciSorgu(int userId)
        {
            // EF Core kodları (ToListAsync vb.) sadece burada yaşar.
            return await _context.Evraklar
                .Include(x => x.EvrakKonuKodu)
                .Include(x => x.Olusturan)
                .Where(e => e.BelgeDurum == Enums.BelgeDurum.Taslak &&
                            e.AkisAdimlari.Any(a => a.KullaniciId == userId && a.SiradakiMi))
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
