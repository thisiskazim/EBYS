using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.MuhatapDTO
{
    public class BireyselMuhatapCreateDTO : BireyselMuhatapBaseDTO
    {
    }
    public class BireyselMuhatapUpdateDTO : BireyselMuhatapBaseDTO
    {
        public int Id { get; set; }
    }
    public class BireyselMuhatapListDTO : BireyselMuhatapBaseDTO
    {
        public int Id { get; set; }
    }
}
