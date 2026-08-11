using EBYS.Domain.Enum;

namespace EBYS.Persistence.Gemini.Instructions
{
    internal static class YaziUzunlukInstructionHelper
    {
        internal static string GetLengthRules(Enums.YaziUzunlugu yaziUzunlugu) =>
            yaziUzunlugu switch
            {
                Enums.YaziUzunlugu.KisaVeOz =>
                    """
                    YAZI UZUNLUĞU: KISA VE ÖZ
                    - konu: Tek cümle, en fazla 15 kelime.
                    - icerik: Tek paragraf; en fazla 3-5 cümle, sadece temel talep veya bilgi.
                    - Gereksiz açıklama, tekrar ve uzun gerekçe kullanma.
                    - icerik alanı tek satırda veya en fazla iki kısa cümle olacak şekilde yaz; JSON formatını bozma.
                    """,
                Enums.YaziUzunlugu.OrtaUzunlukta =>
                    """
                    YAZI UZUNLUĞU: ORTA UZUNLUKTA
                    - konu: 1-2 cümle; konuyu açıkça özetle.
                    - icerik: 2-4 paragraf; giriş, gerekçe ve sonuç dengeli olsun.
                    - Detay ver ama gereksiz uzatma.
                    - Toplam yaklaşık 150-350 kelime hedefle.
                    """,
                Enums.YaziUzunlugu.Uzun =>
                    """
                    YAZI UZUNLUĞU: UZUN
                    - konu: Kapsamlı ama net bir konu cümlesi.
                    - icerik: 4-6 paragraf; arka plan, gerekçe, detay ve sonuç bölümleri olsun.
                    - Talebi/bilgiyi tam ve ikna edici biçimde açıkla.
                    - Toplam yaklaşık 350-600 kelime hedefle.
                    """,
                _ =>
                    """
                    YAZI UZUNLUĞU: ORTA UZUNLUKTA
                    - konu: 1-2 cümle; konuyu açıkça özetle.
                    - icerik: 2-4 paragraf; giriş, gerekçe ve sonuç dengeli olsun.
                    """
            };
    }
}
