using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.Interface;
using EBYS.Domain.Entities;

namespace EBYS.Persistence.Repository   
{
    public class EvrakRepository : GenericRepository<Evrak>, IEvrakRepository
    {
        public EvrakRepository(EBYSContext context) : base(context){ }




    }
}
