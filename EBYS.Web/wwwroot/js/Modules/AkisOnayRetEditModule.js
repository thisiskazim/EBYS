var AkisOnayRedEditModule = (function () {

    var _selectedEvrakId = null;
    var _currentGridSelector = null;
    var _successCallback = null;

    function getEmzaModal() {
        var modalEl = document.getElementById('emzaModal');
        return modalEl ? bootstrap.Modal.getOrCreateInstance(modalEl) : null;
    }

    function closeEmzaModal() {
        getEmzaModal()?.hide();
    }


    return {
        //onayla: function (id, gridSelector, successCallback) {

        //    if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) return;

        //    var $gridEl = $(gridSelector);
        //    if ($gridEl.length) kendo.ui.progress($gridEl, true);

        //    ApiService.postJson("Akis/Onayla/" + id)
        //        .done(function (response) {
        //            showNotification(response.mesaj || "Evrak başarıyla onaylandı.", "success");
        //            if (typeof successCallback === "function") successCallback();
        //        })
        //        .always(function () {
        //            if ($gridEl.length) kendo.ui.progress($gridEl, false);
        //        });
        //},
        // 🚀 İMZALA BUTONUNA BASILINCA TEK NATIVE "confirm" YERİNE E-İMZA MODALI AÇILIYOR

      
        // 🎯 1. İMZALA BUTONUNA BASILDINDA MODALI AÇAR
        onaylaPopUpAc: function (id, gridSelector, successCallback) {
            _selectedEvrakId = id;
            _currentGridSelector = gridSelector;
            _successCallback = successCallback;

            $("#txtEmzaPinKodu").val("");

            var modal = getEmzaModal();
            if (modal) {
                modal.show();

                var $select = $("#cmbSertifikaListesi");
                $select.html('<option value="">Sertifikalar taranıyor...</option>').prop("disabled", true);

                // 🎯 Backend'deki TakiliKartlariGetirAsync metodunu tetikler
                ApiService.getJson("Akis/TakiliKartlariGetir")
                    .done(function (kartlar) {
                        $select.empty();
                        if (kartlar && kartlar.length > 0) {
                            $.each(kartlar, function (index, kart) {
                                $select.append($('<option>', {
                                    value: kart,
                                    text: kart
                                }));
                            });
                            $select.prop("disabled", false);
                        } else {
                            $select.append('<option value="">Takılı e-imza bulunamadı!</option>');
                        }
                    })
                    .fail(function () {
                        $select.html('<option value="">Sertifika listesi alınamadı</option>');
                    });

                setTimeout(function () { $("#txtEmzaPinKodu").focus(); }, 350);
            } else {
                showNotification("E-İmza modal bileşeni sayfada bulunamadı!", "error");
            }
        },

        // 🎯 2. MODAL İÇİNDEKİ "İMZALA VE ONAYLA" BUTONU
        submitEmzaOnay: function () {
            var pinKodu = $("#txtEmzaPinKodu").val();

            if (!pinKodu || pinKodu.trim() === "") {
                showNotification("Lütfen E-İmza PIN kodunuzu giriniz!", "warning");
                return;
            }

            var $gridEl = $(_currentGridSelector);

            closeEmzaModal();

            // Grid üzerinde loading başlat
            if ($gridEl.length) kendo.ui.progress($gridEl, true);

            var url = "Akis/Onayla/" + _selectedEvrakId + "?pinKodu=" + encodeURIComponent(pinKodu);

            ApiService.postJson(url, {})
                .done(function (response) {
                    if (response && response.basariliMi !== false) {
                        showNotification(response.mesaj || "Evrak E-İmza ile başarıyla onaylandı.", "success");
                        if (typeof _successCallback === "function") _successCallback();
                    } else {
                        showNotification(response.mesaj || "Onay işlemi başarısız.", "error");
                    }
                })
                .fail(function (err) {
                    var errMsg = (err && err.responseJSON && err.responseJSON.mesaj)
                        ? err.responseJSON.mesaj
                        : "İşlem sırasında bir sunucu hatası oluştu.";
                    showNotification(errMsg, "error");
                })
                .always(function () {
                    if ($gridEl.length) kendo.ui.progress($gridEl, false);
                });
        },



        reddetPopupAc: function (id, gridSelector, successCallback) {
            kendo.prompt("Lütfen bir reddetme gerekçesi giriniz:", "")
                .done(function (not) {
                    if (not && not.trim() !== "") {

                      
                        var $gridEl = $(gridSelector);
                        if ($gridEl.length) kendo.ui.progress($gridEl, true);

                        var url = "Akis/Reddet/" + id + "?not=" + encodeURIComponent(not);

                        ApiService.postJson(url, {})
                            .done(function (response) {
                                showNotification(response.mesaj || "Evrak başarıyla reddedildi.", "success");
                                if (typeof successCallback === "function") successCallback();
                            })
                            .always(function () {
                                if ($gridEl.length) kendo.ui.progress($gridEl, false);
                            });

                    } else if (not === "") {
                        alert("Reddetme gerekçesi girmek zorunludur!");
                    }
                })
                .fail(function () {
                    console.log("Reddetme işleminden vazgeçildi.");
                });
        },

        iadePopupAc: function (id, gridSelector, successCallback) {
            kendo.prompt("Lütfen bir iade gerekçesi giriniz:", "")
                .done(function (not) {
                    if (not && not.trim() !== "") {

                        var $gridEl = $(gridSelector);
                        if ($gridEl.length) kendo.ui.progress($gridEl, true);

                        var url = "Akis/IadeEt/" + id + "?not=" + encodeURIComponent(not);

                        ApiService.postJson(url, {})
                            .done(function (response) {
                                showNotification(response.mesaj || "Evrak başarıyla iade edildi.", "success");
                                if (typeof successCallback === "function") successCallback();
                            })
                            .always(function () {
                                if ($gridEl.length) kendo.ui.progress($gridEl, false);
                            });

                    } else if (not === "") {
                        alert("İade gerekçesi girmek zorunludur!");
                    }
                })
                .fail(function () {
                    console.log("İade işleminden vazgeçildi.");
                });
        },

    
        edit: function (id) {
            window.location.href = '/GidenEvrak/GidenEvrakOlustur?id=' + id;
        },

        cancel: function (id, gridSelector, successCallback) {
            if (confirm("Bu evrakı silmek istediğinize emin misiniz?")) {
                var $gridEl = $(gridSelector);
                if ($gridEl.length) kendo.ui.progress($gridEl, true);

                ApiService.delete("GidenEvrak/EvrakSil/" + id)
                    .done(function (response) {
                        showNotification(response.mesaj || "Evrak başarıyla silindi.", "success");
                        if (typeof successCallback === "function") successCallback();
                    })
                    .always(function () {
                        if ($gridEl.length) kendo.ui.progress($gridEl, false);
                    });
            }
        }

    };
})();

$(document).ready(function () {

    $(document).on("click", "#btnEmzaOnaylaSubmit", function () {
        AkisOnayRedEditModule.submitEmzaOnay();
    });

    $(document).on("keypress", "#txtEmzaPinKodu", function (e) {
        if (e.which === 13) {
            e.preventDefault();
            AkisOnayRedEditModule.submitEmzaOnay();
        }
    });
});