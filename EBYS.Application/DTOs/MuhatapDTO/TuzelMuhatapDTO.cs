using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.MuhatapDTO
{
    public class TuzelKisiMuhatapCreateDTO : TuzelKisiMuhatapBaseDTO
    {
    }
    public class TuzelKisiMuhatapUpdateDTO : TuzelKisiMuhatapBaseDTO
    {
        public int Id { get; set; }
    }
    public class TuzelKisiMuhatapListDTO : TuzelKisiMuhatapBaseDTO
    {
        public int Id { get; set; }
    }
}
