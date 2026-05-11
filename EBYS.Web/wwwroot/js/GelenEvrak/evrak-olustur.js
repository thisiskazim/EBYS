var EvrakOlustur = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/GelenEvrak/";

    // FormData gönderimi için güncellenmiş AJAX çağrısı
    var _ajaxCallFormData = function (url, formData) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "POST",
            data: formData,
            processData: false, // Jquery'nin veriyi işlemesini engeller (Dosya için şart)
            contentType: false, // Content-Type header'ını otomatik ayarlar (Multipart/form-data)
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
            var bilgiler = GelenEvrakBilgiModule.getData(); // GelenEvrakBaseDTO alanları
            var ilgiler = IlgilerModule.getData();          // List<GelenEvrakIlgiCreateDTO>
            var yanEkler = GelenEklerModule.getData();
            var asilEvrak = GelenOnizlemeModule.getAsilEvrak();// List<GelenEvrakEkCreateDTO>

            var formData = new FormData();

            // temel
            Object.keys(bilgiler).forEach(key => {
                if (bilgiler[key] !== null && bilgiler[key] !== undefined) {
                    formData.append(key, bilgiler[key]);
                }
            });

            //ilgiler
            if (ilgiler != null) {
                ilgiler.forEach((ilgi, index) => {
                    // Giden evrakta sadece IlgiMetni gönderiyorduk, mimariyi bozmuyoruz
                    formData.append(`Ilgiler[${index}].IlgiMetni`, ilgi.IlgiMetni);

                    // Eğer güncelleme ise Id gönderilir
                    if (ilgi.Id > 0) {
                        formData.append(`Ilgiler[${index}].Id`, ilgi.Id);
                    }
                });
            }

  
            var ekIndex = 0;

            if (asilEvrak) {
                formData.append(`Ekler[${ekIndex}].Ad`, "Asıl Evrak"); // GelenEvrakEkCreateDTO.Ad
                formData.append(`Ekler[${ekIndex}].Dosya`, asilEvrak); // GelenEvrakEkCreateDTO.Dosya
                ekIndex++;
            } else {
                showNotification("Lütfen önizleme panelinden asıl evrakı yükleyin!", "error");
                return;
            }

            // Sonra Grid'deki yan ekleri listenin devamına (Ekler[1], Ekler[2]...) ekliyoruz
            if (yanEkler != null) {
                yanEkler.forEach(function (ek) {
                    formData.append(`Ekler[${ekIndex}].Ad`, ek.Ad);
                    if (ek.Dosya) {
                        formData.append(`Ekler[${ekIndex}].Dosya`, ek.Dosya);
                    }
                    // Not: CreateDTO'da Id yok demiştin, o yüzden sadece Ad ve Dosya
                    ekIndex++;
                });
            }

            // 4. Action Belirle ve Gönder
            var action = bilgiler.Id > 0 ? "EvrakGuncelle" : "EvrakOlustur";

            _ajaxCallFormData(action, formData).done(function (response) {
                showNotification("Gelen evrak başarıyla kaydedildi.", "success");
                setTimeout(function () {
                    window.location.href = "/GelenEvrak/Liste";
                }, 1000);
            });
        },

        loadInitialData: function () {
            var urlParams = new URLSearchParams(window.location.search);
            var id = urlParams.get('id') || $("#EvrakId").val();

            if (id && id !== "0" && id !== "") {
     
                $.get(_apiBaseUrl + "EvrakGetir/" + id, function (response) {
                    EvrakBilgiModule.setData(response);
                    IlgilerModule.setData(response.ilgiler);
                    GelenEklerModule.setData(response.ekler);
                });
            }
        }
    };
})();


$(document).ready(function () {
    if (typeof EvrakBilgiModule !== "undefined") EvrakBilgiModule.init();
    if (typeof GelenEklerModule !== "undefined") GelenEklerModule.init();
    if (typeof GelenOnizlemeModule !== "undefined") GelenOnizlemeModule.init();
    if (typeof IlgilerModule !== "undefined") IlgilerModule.init();
  
    EvrakOlustur.init();

    $("#evrakKaydet").on("click", function (e) {
        EvrakOlustur.kaydet();
    });
});