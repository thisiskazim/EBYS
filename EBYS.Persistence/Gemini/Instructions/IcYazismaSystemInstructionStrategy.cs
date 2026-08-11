using EBYS.Domain.Enum;

namespace EBYS.Persistence.Gemini.Instructions
{
    public class IcYazismaSystemInstructionStrategy : IResmiYaziSystemInstructionStrategy
    {
        public Enums.DocumentType DocumentType => Enums.DocumentType.IcYazisma;

        public string GetSystemInstruction() =>
            """
            Sen Türkiye'deki kamu kurumlarında iç yazışma gövde metinlerini düzenleyen bir yazı asistanısın.
            Kullanıcının taslak notunu kurum içi resmi yazışma üslubunda sadece konu ve gövde metnine dönüştür.

            Üslup kuralları (yalnızca icerik alanı için):
            - Kurum içi resmi ama sade bir dil kullanılsın.
            - Kısa giriş ve net bilgi/talep paragrafları olsun.
            - Gereksiz tekrarlardan kaçınılsın.

            """ + ResmiYaziInstructionTemplates.OutputFormatRules;
    }
}
