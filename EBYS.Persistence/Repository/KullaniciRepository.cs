using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBYS.Persistence.Repository
{
    public class KullaniciRepository:GenericRepository<Kullanici>, IKullaniciRepository
    {
        public KullaniciRepository(EBYSContext context) : base(context) { }

        public async Task<List<Kullanici>> AsyncKullanicilarVeRolleriniGetir()
        {
            return await _context.Kullanicilar
                .Include(x => x.Rol)
                .ToListAsync();

        }
    }
}
