using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Interfaces.IService.IGidenEvrakService
{
    public interface IEimzaService
    {
        Task<List<string>> TakiliKartlariGetirAsync();
        Task<byte[]> EvrakImzalaAsync(byte[] pdfBytes, string pinKodu);
    }
}
