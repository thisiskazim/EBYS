using EBYS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.Common.Interface
{
    public interface ITokenService
    {
        string CreateToken(Kullanici kullanici);
    }
}
