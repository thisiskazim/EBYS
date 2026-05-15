using AutoMapper;
using AutoMapper.QueryableExtensions;
using EBYS.Application.DTOs.EvrakDTO;
using EBYS.Application.DTOs.GelenEvrakDTO;
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
    public class GelenEvrakRepository : GenericRepository<GelenEvrak>, IGelenEvrakRepository
    {

        private readonly IMapper _mapper;
        public GelenEvrakRepository(EBYSContext context, IMapper mapper) : base(context) {
            _mapper = mapper;
        }

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

        public async Task<List<GelenEvrakListDTO>> GelenEvrakListAsync()
        {
            return await _context.GelenEvraklar
                    .AsNoTracking()
                    .Where(x => !x.isDelete)
                    .OrderByDescending(x => x.creat_time)
                    .ProjectTo<GelenEvrakListDTO>(_mapper.ConfigurationProvider) // Mermi burada!
                    .ToListAsync();
            //projectto kullanarak direkt olarak veritabanından DTO'ya dönüşüm yapıyoruz, bu sayede gereksiz verilerin çekilmesini engelliyoruz ve performansı artırıyoruz.

        }
    }
}
