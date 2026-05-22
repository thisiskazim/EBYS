var EvrakOlustur = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/GelenEvrak/";

  
    var _ajaxCallFormData = function (url, formData) {
        return $.ajax({
            url: _apiBaseUrl + url,
            type: "POST",
            data: formData,
            processData: false, 
            contentType: false,
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
            var bilgiler = GelenEvrakBilgiModule.getData(); 
            var ilgiler = GelenIlgilerModule.getData();          
            var yanEkler = GelenEklerModule.getData();
            var asilEvrak = GelenOnizlemeModule.getAsilEvrak();

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
                 
                    formData.append(`Ilgiler[${index}].IlgiMetni`, ilgi.IlgiMetni);

                
                    if (ilgi.Id > 0) {
                        formData.append(`Ilgiler[${index}].Id`, ilgi.Id);
                    }
                });
            }

  
            var ekIndex = 0;

            if (asilEvrak) {
                formData.append(`Ekler[${ekIndex}].Ad`, "Üst Yazı"); 
                formData.append(`Ekler[${ekIndex}].Dosya`, asilEvrak); 
                ekIndex++;
            } else {
                showNotification("Lütfen önizleme panelinden asıl evrakı yükleyin!", "error");
                return;
            }

          
            if (yanEkler && Array.isArray(yanEkler) && yanEkler.length > 0) {
                yanEkler.forEach(function (ek) {
                    console.log("Gönderilen Ek:", ek.Ad, "Dosya Nesnesi:", ek.Dosya); 
                    if (ek.Ad) {
                        formData.append(`Ekler[${ekIndex}].Ad`, ek.Ad);
                      
                        if (ek.Dosya) {
                            formData.append(`Ekler[${ekIndex}].Dosya`, ek.Dosya);
                        }
                        ekIndex++;
                    }
                });
            }

         
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
                    GelenEvrakBilgiModule.setData(response);
                    GelenIlgilerModule.setData(response.ilgiler);
                    GelenEklerModule.setData(response.ekler);
                });
            }
        }
    };
})();


$(document).ready(function () {
    if (typeof GelenEvrakBilgiModule !== "undefined") GelenEvrakBilgiModule.init();
    if (typeof GelenEklerModule !== "undefined") GelenEklerModule.init();
    if (typeof GelenOnizlemeModule !== "undefined") GelenOnizlemeModule.init();
    if (typeof GelenIlgilerModule !== "undefined") GelenIlgilerModule.init();
  
    EvrakOlustur.init();

    $("#evrakKaydet").on("click", function (e) {
        EvrakOlustur.kaydet();
    });
});