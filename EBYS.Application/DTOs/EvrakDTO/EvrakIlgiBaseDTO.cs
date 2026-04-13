

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class EvrakIlgiBaseDTO
    {
        public string? IlgiMetni { get; set; }

        //bir gün dosya ekleme ihtiyacı olabilir diye ekledim
       //  public string? EkDosyaYolu { get; set; }
    }

    public class EvrakIlgiCreateDTO : EvrakIlgiBaseDTO
    {
       
    }

    public class EvrakIlgiUpdateDTO : EvrakIlgiBaseDTO
    {
        public int Id { get; set; }
    }


}
