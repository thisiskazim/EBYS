using EBYS.Application.Interfaces.IService.IGidenEvrakService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Services.GidenEvrakService
{//imzalama similasyonu için oluşturuldu. Gerçek imzalama işlemi için ilgili e-imza kütüphanesi ile entegre edilmelidir.
    public class MockEimzaService: IEimzaService
    {
        public Task<List<string>> TakiliKartlariGetirAsync()
        {
            var kartlar = new List<string>
            {
                "TÜBİTAK Kamu SM - Test NES"
            };
            return Task.FromResult(kartlar);
        }

        public async Task<byte[]> EvrakImzalaAsync(byte[] pdfBytes, string pinKodu)
        {
    
            if (pinKodu != "1234")
            {
                throw new Exception("Hatalı E-İmza PIN Kodu! ( PIN: 1234)");
            }

           
            await Task.Delay(1200);

 
            return pdfBytes;
        }
    }
}
