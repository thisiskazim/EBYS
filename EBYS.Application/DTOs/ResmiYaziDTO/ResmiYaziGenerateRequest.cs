using EBYS.Domain.Enum;

namespace EBYS.Application.DTOs.ResmiYaziDTO
{
    public class ResmiYaziGenerateRequest
    {
        public Enums.DocumentType YaziTuru { get; set; }
        public Enums.YaziUzunlugu YaziUzunlugu { get; set; } = Enums.YaziUzunlugu.OrtaUzunlukta;
        public string TaslakMetin { get; set; } = string.Empty;
    }
}
