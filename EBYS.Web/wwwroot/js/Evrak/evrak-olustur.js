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

        updateImzaciFromRota: function (rotaId) {
            if (!rotaId || rotaId === "0") return;

            $.get("https://localhost:7060/api/ImzaRota/ImzaRotaGetir/" + rotaId, function (response) {
       
                var adimlar = response.rotaAdimlari || [];

                if (adimlar && adimlar.length > 0) {

                    var siraliAdimlar = adimlar.sort((a, b) => (a.siraNo || a.id) - (b.siraNo || b.id));
                    var sonAdim = siraliAdimlar[siraliAdimlar.length - 1];
            
                    var ad = sonAdim.adSoyad;
                    var unvan = sonAdim.rolAdi;

         

                    if (typeof OnizlemeModule !== "undefined") {
                        OnizlemeModule.setImzaci(ad, unvan);
                    }
                } 
            });
        },

        kaydet: function () {
            var alicilar = AliciModule.getData();
            var bilgiler = EvrakBilgiModule.getData();
            var ilgiler = IlgilerModule.getData();
            var ekler = EklerModule.getData(); 

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
                formData.append(`Muhataplar[${index}].Adi`, alici.Adi);

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
     
                $.get(_apiBaseUrl + "EvrakGetir/" + id, function (response) {
                    EvrakBilgiModule.setData(response);
                    AliciModule.setData(response.muhataplar);
                    IlgilerModule.setData(response.ilgiler);
                   // if (typeof EklerModule !== "undefined") {
                        EklerModule.setData(response.ekler);
                   // }
                    var rotaId = response.imzaRotaId || response.ImzaRotaId;
                    if (rotaId) {
                        EvrakOlustur.updateImzaciFromRota(rotaId);
                    }
                });
            }
        }
    };
})();


$(document).ready(function () {
    // 1. Modülleri Başlat
    if (typeof AliciModule !== "undefined") AliciModule.init();
    if (typeof EvrakBilgiModule !== "undefined") EvrakBilgiModule.init();
    if (typeof EklerModule !== "undefined") EklerModule.init();
    if (typeof IlgilerModule !== "undefined") IlgilerModule.init();
  

    EvrakOlustur.init();

    var tabEl = document.querySelector('#gorunum-tab');
    if (tabEl) {
        tabEl.addEventListener('shown.bs.tab', function (event) {
            setTimeout(function () {
                OnizlemeModule.verileriYukle();
            }, 150); 
        });
    }


    $("#evrakKaydet").on("click", function (e) {
        EvrakOlustur.kaydet();
    });
});