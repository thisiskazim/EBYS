using EBYS.Application.Interfaces.Repository;
using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Persistence.Repository
{
    public class ImzaRotaRepository:GenericRepository<ImzaRota>,IImzaRotaRepository
    {
        public ImzaRotaRepository(EBYSContext context) : base(context) { }

        public Task<List<ImzaRota>> SilinecekImzaRotaAdimlari()
        {
           
        }

        public async Task<List<ImzaRota>> SilinecekImzaRotaAdimlari(int id, ImzaRota t)
        {
            var getRota = await GetByIdAsync(id, t => t.ImzaRotaAdimlari);

         return getRota.ImzaRotaAdimlari
              .Where(dbAdim => !t.ImzaRotaAdimlari.Any(dtoAdim => dtoAdim.Id == dbAdim.Id))
              .ToList();
        }
    }
}
