using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.MuhatapDTO
{
    public class KurumMuhatapCreateDTO : KurumMuhatapBaseDTO
    {
    }
    public class KurumMuhatapUpdateDTO : KurumMuhatapBaseDTO
    {
        public int Id { get; set; }
    }
    public class KurumMuhatapListDTO : KurumMuhatapBaseDTO
    {
        public int Id { get; set; }
    }

}
