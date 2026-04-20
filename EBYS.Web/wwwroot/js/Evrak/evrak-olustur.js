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
        init: function () {
            this.loadInitialData();
        },  

     

        kaydet: function () {
            // Her modülün kendi verisini topluyoruz
            var alicilar = AliciModule.getData();
            var bilgiler = EvrakBilgiModule.getData();
            //   var ekler=EklerModule.getData();
            var ilgiler = IlgilerModule.getData();
         
            var payload = { ...bilgiler, Muhataplar: alicilar, Ilgiler: ilgiler };
           

            if (!payload.Konu) {
                
                showNotification("Lütfen evrak konusunu giriniz.", "warning");
                return;
            }

            var action = payload.Id > 0 ? "EvrakGuncelle" : "EvrakOlustur";

            _ajaxCall(action, "POST", payload).done(function (response) {
                showNotification("Evrak başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/Akis/ImzaBekleyenListele"; }, 1000);
            });
        },

        loadInitialData: function () {
            var urlParams = new URLSearchParams(window.location.search);
            var id = urlParams.get('id') || $("#EvrakId").val();

            if (id && id !== "0" && id !== "") {
                _ajaxCall("EvrakGetir/" + id, "GET").done(function (response) {
                    EvrakBilgiModule.setData(response);
                    AliciModule.setData(response.muhataplar);
                    IlgilerModule.setData(response.ilgiler);
                });
            }
        }



    }
})();


$(document).ready(function () {
    if (typeof AliciModule !== "undefined") AliciModule.init();
    if (typeof EvrakBilgiModule !== "undefined") EvrakBilgiModule.init();
    // if (typeof EklerModule !== "undefined") EklerModule.init();
    if (typeof IlgilerModule !== "undefined") IlgilerModule.init();

    EvrakOlustur.init();

    // 3. KAYDET BUTONU TIKLANDIĞINDA ÇALIŞTIR
    $("#evrakKaydet").on("click", function (e) {
        EvrakOlustur.kaydet(); // Sadece tıklanınca kaydet
    });
  

});