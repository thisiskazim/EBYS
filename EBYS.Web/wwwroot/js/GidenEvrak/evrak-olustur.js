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
            var ekler = EklerModule.getData(); 
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

          
            ekler.forEach((ek, index) => {
               
                formData.append(`Ekler[${index}].Id`, ek.Id || 0);
                formData.append(`Ekler[${index}].Ad`, ek.Ad);

           
                if (ek.Dosya) {
                    formData.append(`Ekler[${index}].Dosya`, ek.Dosya);
                }
            });

       
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