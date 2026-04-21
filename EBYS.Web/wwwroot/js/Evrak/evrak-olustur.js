var EvrakOlustur = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/Evrak/";

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
            var alicilar = AliciModule.getData();
            var bilgiler = EvrakBilgiModule.getData();
            var ilgiler = IlgilerModule.getData();
            var ekler = EklerModule.getData(); // Ekler modülünden veriyi çekiyoruz

            // 1. JSON yerine FormData nesnesi oluşturuyoruz
            var formData = new FormData();

            // 2. Temel Bilgileri ekle (EvrakBilgiModule'den gelen tüm alanlar)
            Object.keys(bilgiler).forEach(key => {
                if (bilgiler[key] !== null) formData.append(key, bilgiler[key]);
            });

            // 3. Muhatapları/Alıcıları ekle (Liste olduğu için döngüyle append ediyoruz)
            alicilar.forEach((alici, index) => {
                formData.append(`Muhataplar[${index}].MuhatapId`, alici.MuhatapId);
                formData.append(`Muhataplar[${index}].IsBilgi`, alici.IsBilgi);
            });

            // 4. İlgileri ekle
            ilgiler.forEach((ilgi, index) => {
                formData.append(`Ilgiler[${index}].IlgiMetni`, ilgi.IlgiMetni);
            });

            // 5. EKLERİ EKLE (Kritik Nokta)
            ekler.forEach((ek, index) => {
                // Eğer güncelleme ise Id değerini gönder (0 veya gerçek Id)
                formData.append(`Ekler[${index}].Id`, ek.Id || 0);
                formData.append(`Ekler[${index}].Ad`, ek.Ad);

                // Eğer yeni bir dosya seçilmişse (DosyaObj varsa) ekle
                if (ek.Dosya) {
                    formData.append(`Ekler[${index}].Dosya`, ek.Dosya);
                }
            });

            if (!bilgiler.Konu) {
                showNotification("Lütfen evrak konusunu giriniz.", "warning");
                return;
            }

            var action = bilgiler.Id > 0 ? "EvrakGuncelle" : "EvrakOlustur";

            // Ajax çağrısını yap
            _ajaxCallFormData(action, formData).done(function (response) {
                showNotification("Evrak başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/Akis/ImzaBekleyenListele"; }, 1000);
            });
        },

        loadInitialData: function () {
            var urlParams = new URLSearchParams(window.location.search);
            var id = urlParams.get('id') || $("#EvrakId").val();

            if (id && id !== "0" && id !== "") {
                // GET işlemi JSON döneceği için eski usul kullanılabilir 
                // ya da basit bir $.get yapılabilir.
                $.get(_apiBaseUrl + "EvrakGetir/" + id, function (response) {
                    EvrakBilgiModule.setData(response);
                    AliciModule.setData(response.muhataplar);
                    IlgilerModule.setData(response.ilgiler);
                    if (typeof EklerModule !== "undefined") {
                        EklerModule.setData(response.ekler);
                    }
                });
            }
        }
    };
})();


$(document).ready(function () {
    if (typeof AliciModule !== "undefined") AliciModule.init();
    if (typeof EvrakBilgiModule !== "undefined") EvrakBilgiModule.init();
    if (typeof EklerModule !== "undefined") EklerModule.init(); // Artık aktif!
    if (typeof IlgilerModule !== "undefined") IlgilerModule.init();

    EvrakOlustur.init();

    $("#evrakKaydet").on("click", function (e) {
        EvrakOlustur.kaydet();
    });
});