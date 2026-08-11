using EBYS.Domain.Enum;

namespace EBYS.Application.DTOs.ResmiYaziDTO
{
    public class ResmiYaziGenerateResponse
    {
        public Enums.DocumentType YaziTuru { get; set; }
        public string Konu { get; set; } = string.Empty;
        public string ResmiMetin { get; set; } = string.Empty;
    }
}
