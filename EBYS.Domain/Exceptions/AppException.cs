using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Domain.Exceptions
{

    public class EvrakBulunamadi() : NotFoundException("Evrak Bulunamadı.");
    public class EvrakHareketiBulunamadi() : NotFoundException("Evrak Hareketi Bulunamadı.");
    public class KullaniciBulunamadi() : NotFoundException("Aranan kullanıcı sistemde kayıtlı değil.");
    public class EvrakZatenSevkEdilmis() : BusinessException("Bu evrakın sevk işlemi daha önce zaten yapılmış.");
    public class ImzaRotasıBos() : BusinessException("İmza Rota Adımları Boş. 'İmza Rotası' Menüsünden Doldurunuz");
}
