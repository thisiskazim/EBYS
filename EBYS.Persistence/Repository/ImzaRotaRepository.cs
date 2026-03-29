using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EBYS.Persistence.Repository
{
    public class ImzaRotaRepository:GenericRepository<ImzaRota>,IImzaRotaRepository
    {
        public ImzaRotaRepository(EBYSContext context) : base(context) { }

        public async Task<ImzaRota> GetImzaRotaVeAdimlariDetay(int id)
        {
            return await _context.ImzaRotalar
                .Include(x => x.ImzaRotaAdimlari)
                .ThenInclude(a => a.Kullanici)
                .ThenInclude(k => k.Rol)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
