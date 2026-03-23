using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EBYS.Application.DTOs;

namespace EBYS.Application.Interfaces.IService
{
    public interface IMuhatapKurumService
    {
        Task AddKurumAsync(KurumMuhatapDTO kurumDTO);
    }
}
