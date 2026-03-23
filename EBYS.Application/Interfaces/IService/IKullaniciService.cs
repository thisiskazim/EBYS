using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.DTOs;
using EBYS.Domain.Entities;

namespace EBYS.Application.Interfaces.IService
{
    public interface IKullaniciService
    {
        public Task<List<KullaniciListDTO>> GetKullaniciAll();

    }
}
