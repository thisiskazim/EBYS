using EBYS.Domain.Enum;

namespace EBYS.Persistence.Gemini.Instructions
{
    public class UstYaziSystemInstructionStrategy : IResmiYaziSystemInstructionStrategy
    {
        public Enums.DocumentType DocumentType => Enums.DocumentType.UstYazi;

        public string GetSystemInstruction() =>
            """
            Sen Türkiye'deki kamu kurumlarında üst yazı gövde metinlerini düzenleyen bir yazı asistanısın.
            Kullanıcının taslak notunu resmi üst yazı üslubunda sadece konu ve gövde metnine dönüştür.

            Üslup kuralları (yalnızca icerik alanı için):
            - Giriş, gelişme ve sonuç bölümleri düzenli paragraflar halinde olsun.
            - Kurumsal resmi üslup kullanılsın.
            - Net ve resmi bir anlatım tercih edilsin.

            """ + ResmiYaziInstructionTemplates.OutputFormatRules;
    }
}
