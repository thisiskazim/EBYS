var module = (function () {

    var _fillForm = function (data) {
        if (!data) return;

        $("#Adi").data("kendoTextBox").value(data.adi);
        $("#Telefon").data("kendoTextBox").value(data.telefon);
        $("#KimlikNo").data("kendoTextBox").value(data.kimlikNo);
        $("#Adress").data("kendoTextBox").value(data.adress);
        $("#EPosta").data("kendoTextBox").value(data.ePosta);
    };

    return {
        init: function () {
            this.loadInitialData();
        },

        loadInitialData: function () {
            var id = parseInt($("#Id").val(), 10) || 0;
            if (id > 0) {
                ApiService.getJson("BireyselMuhatap/Getir/" + id).done(function (response) {
                    _fillForm(response);
                });
            }
        },

        kaydet: function () {
            var id = $("#Id").val();
            var data = {
                Id: id === "" ? 0 : parseInt(id),
                Adi: $("#Adi").val(),
                KimlikNo: $("#KimlikNo").val(),
                Adress: $("#Adress").val(),
                Telefon: $("#Telefon").val(),
                EPosta: $("#EPosta").val(),
            };

            var url = data.Id > 0 ? "BireyselMuhatap/Guncelle" : "BireyselMuhatap/Ekle";

            ApiService.postJson(url, data).done(function (res) {
                showNotification("Başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/BireyselMuhatap/Listele"; }, 1000);
            });
        }
    };
})();

$(document).ready(function () {
    module.init();
});
