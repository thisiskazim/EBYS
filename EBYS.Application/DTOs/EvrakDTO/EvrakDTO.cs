
using EBYS.Domain.Enum;

using EBYS.Application.DTOs.EvtakDTO;

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class GidenEvrakBaseDTO
    {
        public string Konu { get; set; }
        public int KonuKoduId { get; set; }
        public string Icerik { get; set; }
        public string? ImzaAltindaOlanIcerik { get; set; }
        public Enums.GizlilikDerecesi GizlilikDerecesi { get; set; }
        public Enums.IvedilikDerecesi IvedilikDerecesi { get; set; }
        public int ImzaRotaId { get; set; }
        public List<EvrakMuhatapSecimDTO> Muhataplar { get; set; } = new();

    }

    public class GidenEvrakCreateDTO : GidenEvrakBaseDTO
    {
        public List<EvrakIlgiCreateDTO>? Ilgiler { get; set; } = new();
        public List<EvrakEkCreateDTO>? Ekler { get; set; } = new();
    }
    public class GidenEvrakUpdateDTO : GidenEvrakBaseDTO
    {
        public int Id { get; set; }
        public List<EvrakIlgiUpdateDTO>? Ilgiler { get; set; } = new();
        public List<EvrakEkUpdateDTO>? Ekler { get; set; } = new();
    }

    public class GidenEvrakListDTO : GidenEvrakBaseDTO
    {
      //dolacak 
    }
}
