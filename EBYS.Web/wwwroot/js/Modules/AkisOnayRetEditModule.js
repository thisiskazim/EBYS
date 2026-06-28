var AkisOnayRedEditModule = (function () {
    return {
        onayla: function (id, gridSelector, successCallback) {
           
            if (!confirm("Seçili evrakı onaylamak istediğinize emin misiniz?")) return;

            var $gridEl = $(gridSelector);
            if ($gridEl.length) kendo.ui.progress($gridEl, true);

            ApiService.postJson("Akis/Onayla/" + id)
                .done(function (response) {
                    showNotification(response.mesaj || "Evrak başarıyla onaylandı.", "success");
                    if (typeof successCallback === "function") successCallback();
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