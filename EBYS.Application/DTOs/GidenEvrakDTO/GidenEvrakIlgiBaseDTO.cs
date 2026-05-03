

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class GidenEvrakIlgiBaseDTO
    {
        public string? IlgiMetni { get; set; }

        //dosya ekleme ihtiyacı olabilir diye ekledim
       //  public string? EkDosyaYolu { get; set; }
    }

    public class EvrakIlgiCreateDTO : GidenEvrakIlgiBaseDTO
    {
       
    }

    public class EvrakIlgiUpdateDTO : GidenEvrakIlgiBaseDTO
    {
        public int Id { get; set; }
    }


}
