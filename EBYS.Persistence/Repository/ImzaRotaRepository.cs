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
    }
}
