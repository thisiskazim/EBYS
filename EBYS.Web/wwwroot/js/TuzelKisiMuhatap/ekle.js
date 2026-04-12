var module = (function () {
    var _apiBaseUrl = "https://localhost:7060/api/TuzelKisiMuhatap/";

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

    // FORM DOLDURMA (Update için)
    var _fillForm = function (data) {
        if (!data) return;

        // Kendo TextBox değerini set etme yolu budur:

        $("#Adi").data("kendoTextBox").value(data.adi);
        $("#Telefon").data("kendoTextBox").value(data.telefon);
        $("#VergiNo").data("kendoTextBox").value(data.detsisNo);
        $("#VergiDairesi").data("kendoTextBox").value(data.TuzelKodu);
        $("#MersisNo").data("kendoTextBox").value(data.kepAdresi);
        $("#Adress").data("kendoTextBox").value(data.adress);
        $("#EPosta").data("kendoTextBox").value(data.ePosta);
    };

    return {
        init: function () {
            this.loadInitialData();
            this.kaydet();
        },

        loadInitialData: function () {
            var id = $("#Id").val();
            if (id && id !== "0") {
                $("#btnKaydet").text("Güncelle");
                _ajaxCall("Getir/" + id, "GET").done(function (response) {
                    _fillForm(response);
                });
            } else {
                $("#btnKaydet").text("Kaydet");
            }
        },

        kaydet: function () {
            var id = $("#Id").val();
            var formData = {
                Id: id === "" ? 0 : parseInt(id),
                Adi: $("#Adi").val(),
                VergiNo: $("#VergiNo").val(),
                VergiDairesi: $("#VergiDairesi").val(),
                MersisNo: $("#MersisNo").val(),
                Adress: $("#Adress").val(),
                Telefon: $("#Telefon").val(),
                EPosta: $("#EPosta").val(),

            };

            if (!formData.Adi) {
                showNotification("Tuzel adı zorunludur!", "warning");
                return;
            }

            var url = formData.Id > 0 ? "Guncelle" : "Ekle";

            _ajaxCall(url, "POST", formData).done(function (res) {
                showNotification("Başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/TuzelKisiMuhatap/Listele"; }, 1500);
            });
        }
    };
})();

$(document).ready(function () {
    module.init();
});