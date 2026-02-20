using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.Interface;
using EBYS.Domain.Entities;

namespace EBYS.Persistence.Repository
{
    public class MuhatapRepository:GenericRepository<Muhatap>, IMuhatapRepository
    {
        public MuhatapRepository(EBYSContext context) : base(context) { }
    }
}
