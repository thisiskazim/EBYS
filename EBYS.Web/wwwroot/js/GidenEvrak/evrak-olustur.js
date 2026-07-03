var EvrakOlustur = (function () {
    return {
        init: function () {
            this.loadInitialData();
        },  

        updateImzaciFromRota: function (rotaId) {
            if (!rotaId || rotaId === "0") return;
            ApiService.getJson("ImzaRota/ImzaRotaGetir/" + rotaId).done(function (response) {
       
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
            var yanEkler = EklerModule.getData(); // Kullanıcının eklediği ek dosyalar
           

            var formData = new FormData();


            Object.keys(bilgiler).forEach(key => {
                if (bilgiler[key] !== null) formData.append(key, bilgiler[key]);
            });

            alicilar.forEach((alici, index) => {
                formData.append(`Muhataplar[${index}].MuhatapId`, alici.MuhatapId);
                formData.append(`Muhataplar[${index}].IsBilgi`, alici.IsBilgi);
                formData.append(`Muhataplar[${index}].Adi`, alici.Adi);

            });

        
            ilgiler.forEach((ilgi, index) => {
                formData.append(`Ilgiler[${index}].IlgiMetni`, ilgi.IlgiMetni);
            });

            var ekIndex = 0;

           
            var asilUstYazi = OnizlemeModule.getGeneratedPdfFile();
            if (asilUstYazi) {
                formData.append(`Ekler[${ekIndex}].Id`, 0);
                formData.append(`Ekler[${ekIndex}].Ad`, "Üst Yazı");
                formData.append(`Ekler[${ekIndex}].Dosya`, asilUstYazi);
                formData.append(`Ekler[${ekIndex}].IsAsilEvrak`, true);
                ekIndex++;
            } else {
                showNotification("Lütfen önce evrak görünümünü oluşturup önizleyin!", "error");
                return;
            }


            if (yanEkler && Array.isArray(yanEkler)) {
                yanEkler.forEach((ek) => {
                    formData.append(`Ekler[${ekIndex}].Id`, ek.Id || 0);
                    formData.append(`Ekler[${ekIndex}].Ad`, ek.Ad);
                    // 🚀 KRİTİK DÜZELTME: Bu dosyaların yan ek olduğunu backend'e açıkça söylüyoruz
                    formData.append(`Ekler[${ekIndex}].IsAsilEvrak`, false);
                    if (ek.Dosya) {
                        formData.append(`Ekler[${ekIndex}].Dosya`, ek.Dosya);
                    }
                    ekIndex++;
                });
            }
          


       
            var action = bilgiler.Id > 0 ? "GidenEvrak/EvrakGuncelle" : "GidenEvrak/EvrakOlustur";

            ApiService.postFormData(action, formData).done(function () {
                showNotification("Evrak başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/GidenEvrakAkis/ImzaBekleyenListele"; }, 1000);
            });
        },

        loadInitialData: function () {
            var urlParams = new URLSearchParams(window.location.search);
            var id = urlParams.get('id') || $("#EvrakId").val();

            if (id && id !== "0" && id !== "") {
     
                ApiService.getJson("GidenEvrak/EvrakGetir/" + id).done(function (response) {
                    EvrakBilgiModule.setData(response);
                    AliciModule.setData(response.muhataplar);
                    IlgilerModule.setData(response.ilgiler);
                    EklerModule.setData(response.ekler);
          
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