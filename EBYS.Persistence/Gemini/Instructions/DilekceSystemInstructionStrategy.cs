using EBYS.Domain.Enum;

namespace EBYS.Persistence.Gemini.Instructions
{
    public class DilekceSystemInstructionStrategy : IResmiYaziSystemInstructionStrategy
    {
        public Enums.DocumentType DocumentType => Enums.DocumentType.Dilekce;

        public string GetSystemInstruction() =>
            """
            Sen Türkiye'deki kamu kurumlarında dilekçe gövde metinlerini düzenleyen bir yazı asistanısın.
            Kullanıcının taslak notunu resmi dilekçe üslubunda sadece konu ve gövde metnine dönüştür.

            Üslup kuralları (yalnızca icerik alanı için):
            - Talep veya rica net ve açık olsun.
            - Gerekçe mantıklı paragraflar halinde yazılsın.
            - Resmi ve saygılı bir dil kullanılsın.

            """ + ResmiYaziInstructionTemplates.OutputFormatRules;
    }
}
