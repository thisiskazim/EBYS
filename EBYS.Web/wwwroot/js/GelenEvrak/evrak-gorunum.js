var GelenOnizlemeModule = (function () {
    var _asilEvrakFile = null;

    return {

        init: function () {
            console.log("Onizleme Modülü Hazırlanıyor..."); // Bunu görüyorsan init çalışıyordur
            var self = this;
            $("#AsilEvrakDosya").off("change").on("change", function () {
                console.log("Dosya seçimi yakalandı!"); // Dosya seçince bunu görmen lazım
                self.dosyaSecildi(this);
            });
        },
        // Dosya seçildiğinde çalışan o kritik fonksiyon
        dosyaSecildi: function (input) {
            if (input.files && input.files[0]) {
                _asilEvrakFile = input.files[0];

                // 1. Mesajı gizle, Iframe'i göster
                $("#no-file-message").hide();
                $("#pdf-preview-frame").show();

                // 2. Dosyayı yükle
                var fileURL = URL.createObjectURL(_asilEvrakFile);
                $("#pdf-preview-frame").attr("src", fileURL);

                if (typeof showNotification !== "undefined") {
                    showNotification("Üst yazı önizlemeye yüklendi.", "info");
                }
            }
        },

        // Kaydetme anında dosyayı almak için
        getAsilEvrak: function () {
            return _asilEvrakFile;
        }
    };
})();

$(document).ready(function () {
    GelenOnizlemeModule.init();
});