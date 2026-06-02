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
using static EBYS.Domain.Enum.Enums;

namespace EBYS.Persistence.Repository
{
    public class GelenEvrakRepository : GenericRepository<GelenEvrak>, IGelenEvrakRepository
    {

        private readonly IMapper _mapper;
        public GelenEvrakRepository(EBYSContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<int> KayitNumarasiOlustur(int yil)
        {
            return await _context.GelenEvraklar.CountAsync(x => x.EvrakTarihi.Year == yil);
        }

        public async Task<GelenEvrak> DetayliGetirByIdAsync(int id)
        {
            return await _context.GelenEvraklar
                 .Include(x => x.Ilgileri)
                 .Include(x => x.Ekler)
                 .Include(x => x.Sevkler)
                 .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<GelenEvrakListDTO>> FiltreliEvrakGetirAsync(int? currentUserId, GelenEvrakDurumu? evrakDurumu)
        {

            var query = _context.GelenEvraklar.Where(x => !x.isDelete);

            if (evrakDurumu.HasValue)
            {
                switch (evrakDurumu.Value)
                {
     
                    case GelenEvrakDurumu.TeslimAlindi:
                        query = query.Where(x => x.Sevkler
                            .OrderByDescending(s => s.SevkTarihi)
                            .FirstOrDefault().AlanKullaniciId == currentUserId);
                        break;

                    case GelenEvrakDurumu.IadeEdildi:
                        query = query.Where(x => x.Sevkler
                            .OrderByDescending(s => s.SevkTarihi)
                            .FirstOrDefault().GelenEvrakDurumEnum == GelenEvrakDurumu.IadeEdildi
                            && x.Sevkler.OrderByDescending(s => s.SevkTarihi).FirstOrDefault().AlanKullaniciId == null);
                        break;

                    case GelenEvrakDurumu.Cevaplandi:
                        query = query.Where(x => x.Sevkler
                            .OrderByDescending(s => s.SevkTarihi)
                            .FirstOrDefault().GelenEvrakDurumEnum == GelenEvrakDurumu.Cevaplandi);
                        break;

                }
            }



            return await query
             .OrderByDescending(x => x.creat_time)
             .ProjectTo<GelenEvrakListDTO>(_mapper.ConfigurationProvider)
             .ToListAsync();
            //projectto kullanarak direkt olarak veritabanından DTO'ya dönüşüm yapıyoruz, bu sayede gereksiz verilerin çekilmesini engelliyoruz ve performansı artırıyoruz.

        }

        public async Task<GelenEvrakEk> GelenEvrakEkDosyaByIdAsync(int ekId)
        {
            return await _context.GelenEvrakEkler
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Id == ekId);
        }

        public async Task<List<GelenEvrakSevkListDTO>> GelenEvrakSevkHareketleriAsync(int gelenEvrakId)
        {
                    return await _context.GelenEvrakSevkler 
                            .Where(x => x.GelenEvrakId == gelenEvrakId && !x.isDelete) 
                            .OrderBy(x => x.SevkTarihi) 
                            .AsNoTracking()
                            .ProjectTo<GelenEvrakSevkListDTO>(_mapper.ConfigurationProvider) 
                            .ToListAsync();
        }

        public async Task<GelenEvrakSevk> SevkGetirByIdAsync(int gelenEvrakId)
        {
            return await _context.GelenEvrakSevkler
                    .Where(x => x.GelenEvrakId == gelenEvrakId && x.AlanKullaniciId == null && !x.isDelete)
                    .OrderByDescending(x => x.SevkTarihi)
                    .FirstOrDefaultAsync();
        }
    }
}
