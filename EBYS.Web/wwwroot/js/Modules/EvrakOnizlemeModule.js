var EvrakOnizlemeModule = (function () {
    var _dialog = null;
    var _apiBaseUrl = "https://localhost:7060/api/";
    var _init = function () {
        if (!$("#onizlemeDialog").data("kendoDialog")) {
            _onizlemeDialog = $("#onizlemeDialog").kendoDialog({
                width: "1300px",
                height: "800px", // İçerideki 720px'lik d-flex'i rahat taşısın
                title: "Evrak Detay Önizleme",
                closable: true,
                modal: true,
                visible: false,
                actions: [{ text: "Kapat" }]
            }).data("kendoDialog");


        }
    };

    return {
        ac: function (ekId, evrakTipi) {
            _init();
            var self = this;
            var dialog = $("#onizlemeDialog").data("kendoDialog");

            // 1. Önce Dialog'u açıyoruz
            dialog.open();
            $("#onizleme-yukleniyor").show();
            $("#pdf-frame-popup").attr("src", "about:blank"); // Sıfırla

            var url = _apiBaseUrl+ "EvrakOnizleme/EkGoruntule/" + evrakTipi + "/" + ekId;


            // 2. Veriyi çekiyoruz
            var xhr = new XMLHttpRequest();
            xhr.open("GET", url, true);
            xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
            xhr.responseType = "blob";

            xhr.onload = function () {
                if (this.status === 200) {
                    var blob = new Blob([this.response], { type: 'application/pdf' });
                    var fileURL = URL.createObjectURL(blob);

                    // Dialog zaten açık, ama render için milisaniyelik bir es veriyoruz
                    setTimeout(function () {
                        var $iframe = $("#pdf-frame-popup");
                        // URL'nin sonuna fit ekleyerek tam sığmasını sağlayalım
                        $iframe.attr("src", fileURL + "#view=FitH");
                        $("#onizleme-yukleniyor").hide();
                    }, 100);
                }
            };
            xhr.send();
        },
    };
})();


//tekilDosyaOnizle: function (ekId) {
//    var self = this;
//    var dialog = $("#onizlemeDialog").data("kendoDialog");

//    // 1. Önce Dialog'u açıyoruz
//    dialog.open();
//    $("#onizleme-yukleniyor").show();
//    $("#pdf-frame-popup").attr("src", "about:blank"); // Sıfırla

//    // 2. Veriyi çekiyoruz
//    var xhr = new XMLHttpRequest();
//    xhr.open("GET", _apiBaseUrl + "EvrakPdfGoruntule/" + ekId, true);
//    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
//    xhr.responseType = "blob";

//    xhr.onload = function () {
//        if (this.status === 200) {
//            var blob = new Blob([this.response], { type: 'application/pdf' });
//            var fileURL = URL.createObjectURL(blob);

//            // Dialog zaten açık, ama render için milisaniyelik bir es veriyoruz
//            setTimeout(function () {
//                var $iframe = $("#pdf-frame-popup");
//                // URL'nin sonuna fit ekleyerek tam sığmasını sağlayalım
//                $iframe.attr("src", fileURL + "#view=FitH");
//                $("#onizleme-yukleniyor").hide();
//            }, 100);
//        }
//    };
//    xhr.send();
//},









//ac: function (ekId, evrakTipi) {
//    var _dialog = $("#onizlemeDialog").data("kendoDialog");
//    _init();
//    _dialog.open();
//    _dialog.center();

//    $("#onizleme-yukleniyor").show();
//    $("#pdf-frame-popup").attr("src", "about:blank");


//    var url = "/api/EvrakOnizleme/EkGoruntule/" + evrakTipi + "/" + ekId;

//    var xhr = new XMLHttpRequest();
//    xhr.open("GET", url, true);
//    xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
//    xhr.responseType = "blob";

//    xhr.onload = function () {
//        $("#onizleme-yukleniyor").hide();
//        if (this.status === 200) {
//            var blob = new Blob([this.response], { type: 'application/pdf' });
//            var fileURL = URL.createObjectURL(blob);

//            setTimeout(function () {
//                $("#pdf-frame-popup").attr("src", fileURL + "#view=FitH");
//            }, 100);
//        } else {
//            alert("Dosya yüklenemedi!");
//        }
//    };
//    xhr.send();
//}