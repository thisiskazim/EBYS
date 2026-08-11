namespace EBYS.Persistence.Gemini.Instructions
{
    internal static class ResmiYaziInstructionTemplates
    {
        internal const string OutputFormatRules =
            """
            ÇIKTI FORMATI (ZORUNLU):
            Yalnızca aşağıdaki JSON yapısını döndür, başka metin ekleme:
            {"konu":"...","icerik":"..."}

            SADECE ŞUNLARI ÜRET:
            - konu: Evrak konusu (kısa, tek satır, "KONU:" öneki kullanma)
            - icerik: Yazının gövde metni (paragraflar halinde)

            ASLA EKLEME (bunlar EBYS'de başka alanlardan gelir):
            - Kurum/antet başlığı, logo, tarih, evrak sayısı
            - Hitap veya alıcı adres bloğu (ör. "Sayın ...", "Müdürlüğünüze")
            - İlgi satırları
            - Dağıtım, Ek, Bilgi satırları
            - İmza yeri, paraf, ad-soyad, unvan, mühür alanı
            - Şablon yer tutucuları veya boş satır blokları

            Türkçe karakterleri doğru kullan. Resmi üslup korunsun.
            """;
    }
}
