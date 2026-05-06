using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities.GelenEvrak;
using EBYS.Domain.Entities.GidenEvrak;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence.Repository
{
    public class GelenEvrakRepository: GenericRepository<GelenEvrak>, IGelenEvrakRepository
    {

        public GelenEvrakRepository(EBYSContext context) : base(context) { }

        public async Task<int> KayitNumarasiOlustur(int yil)
        {
            return await _context.GelenEvraklar.CountAsync(x => x.DefterTarihi.Year == yil);
        }

        public async Task<GelenEvrak> DetayliGetirAsync(int id)
        {
            return await _context.GelenEvraklar
                 .Include(x => x.Ilgileri)
                 .Include(x => x.Ekler)
                 .Include(x => x.Sevkler)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

       
    }
}
