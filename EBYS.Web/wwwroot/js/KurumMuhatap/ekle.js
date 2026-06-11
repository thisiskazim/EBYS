var module = (function () {

    var _fillForm = function (data) {
        if (!data) return;

        $("#Adi").data("kendoTextBox").value(data.adi);
        $("#Telefon").data("kendoTextBox").value(data.telefon);
        $("#DetsisNo").data("kendoTextBox").value(data.detsisNo);
        $("#KurumKodu").data("kendoTextBox").value(data.kurumKodu);
        $("#KepAdresi").data("kendoTextBox").value(data.kepAdresi);
        $("#Adress").data("kendoTextAreas").value(data.adress);
        $("#EPosta").data("kendoTextBox").value(data.ePosta);
    };

    return {
        init: function () {
            this.loadInitialData();
            
        },

        loadInitialData: function () {
            var id = parseInt($("#Id").val(), 10) || 0;
            if (id > 0) {
                ApiService.getJson("KurumMuhatap/Getir/" + id).done(function (response) {
                    _fillForm(response);
                });
            }
        },

        kaydet: function () {
            var id = $("#Id").val();
            var data = {
                Id: id === "" ? 0 : parseInt(id),
                Adi: $("#Adi").val(),
                DetsisNo: $("#DetsisNo").val(),
                KurumKodu: $("#KurumKodu").val(),
                KepAdresi: $("#KepAdresi").val(),
                Adress: $("#Adress").val(),
                Telefon: $("#Telefon").val(),
                EPosta: $("#EPosta").val(),

            };

            var url = data.Id > 0 ? "KurumMuhatap/Guncelle" : "KurumMuhatap/Ekle";

            ApiService.postJson(url,data).done(function (res) {
                showNotification("Başarıyla kaydedildi.", "success");
                setTimeout(function () { window.location.href = "/KurumMuhatap/Listele"; }, 1000);
            });
        }
    };
})();

$(document).ready(function () {
    module.init();
});