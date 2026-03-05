using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Common.Interface
{
    public interface ICurrentUserService
    {
        int GetKurumId();
        int GetUserId();
    }
}
