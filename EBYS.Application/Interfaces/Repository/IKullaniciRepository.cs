using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Domain.Entities;

namespace EBYS.Application.Interfaces.Repository
{
    public interface IKullaniciRepository:IGenericRepository<Kullanici>
    {
        Task<List<Kullanici>> AsyncKullanicilarVeRolleriniGetir();
    }
}
