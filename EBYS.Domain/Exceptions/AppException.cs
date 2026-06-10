using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Exceptions
{
    // --- NOT FOUND (404) HATALARI ---
    public class EvrakBulunamadi() : NotFoundException("Evrak Bulunamadı.");
    public class EvrakHareketiBulunamadi() : NotFoundException("Evrak Hareketi Bulunamadı.");
    public class KullaniciBulunamadi() : NotFoundException("Aranan kullanıcı sistemde kayıtlı değil.");

    // --- BUSINESS (400) HATALARI ---
    public class EvrakZatenSevkEdilmis() : BusinessException("Bu evrakın sevk işlemi daha önce zaten yapılmış.");
}
