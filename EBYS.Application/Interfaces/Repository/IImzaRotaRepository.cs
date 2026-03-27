using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IImzaRotaRepository:IGenericRepository<ImzaRota>
    {
        Task<List<ImzaRota>> SilinecekImzaRotaAdimlari(int id,ImzaRota t);
    }
}
