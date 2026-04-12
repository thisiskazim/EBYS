var EvrakOlustur = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/Evrak/";
    var _ajaxCall = function (url, type, data) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: type,
            contentType: "application/json",
            data: data ? JSON.stringify(data) : null,
            error: function (err) {
                var msg = err && err.responseText ? err.responseText : "Hata oluştu.";
                showNotification(msg, "error");
            }
        });
    };

    return {
        kaydet: function () {
            // Her modülün kendi verisini topluyoruz
            var alicilar = AliciModule.getData();
            var bilgiler = EvrakBilgiModule.getData();
            //   var ekler=EklerModule.getData();
            var ilgiler = IlgilerModule.getData();
            // Payload oluşturup AJAX ile gönderiyoruz
            var payload = { ...bilgiler, Muhataplar: alicilar, Ilgiler: ilgiler };
            console.log("Gönderilecek Veri:", payload);

            if (!payload.Konu) {
                showNotification("Lütfen evrak konusunu giriniz.", "warning");
                return;
            }
            _ajaxCall("EvrakOlustur", "POST", payload).done(function (response) {
                showNotification("Evrak başarıyla oluşturuldu.", "success");
                // İsterseniz formu temizleyebilir veya başka bir sayfaya yönlendirebilirsiniz
            });
        }

    }
})();


$(document).ready(function () {
    if (typeof AliciModule !== "undefined") AliciModule.init();
    if (typeof EvrakBilgiModule !== "undefined") EvrakBilgiModule.init();
    // if (typeof EklerModule !== "undefined") EklerModule.init();
    if (typeof IlgilerModule !== "undefined") IlgilerModule.init();

    $("#evrakKaydet").on("click", function () {
        EvrakOlustur.kaydet();
    });

});