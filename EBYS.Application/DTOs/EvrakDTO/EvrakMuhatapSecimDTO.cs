using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakMuhatapSecimDTO
    {
        public int MuhatapId { get; set; }
        public bool IsBilgi { get; set; } //bilgi mi gereği mi
        public string Adi { get; set; }
    }
}
