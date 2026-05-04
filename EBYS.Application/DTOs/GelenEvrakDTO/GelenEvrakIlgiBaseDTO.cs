

namespace EBYS.Application.DTOs.EvrakDTO
{
    public class GelenEvrakIlgiBaseDTO
    {
        public string? IlgiMetni { get; set; }

        //dosya ekleme ihtiyacı olabilir diye ekledim
       //  public string? EkDosyaYolu { get; set; }
    }

    public class GelenEvrakIlgiCreateDTO : GelenEvrakIlgiBaseDTO
    {
       
    }

    public class GelenEvrakIlgiUpdateDTO : GelenEvrakIlgiBaseDTO
    {
        public int Id { get; set; }
    }


}
