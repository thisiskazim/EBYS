var EvrakBilgiModule = (function () {
    return {
        init: function () {

            var ddlImzaRota = $("#ImzaRota").data("kendoDropDownList");
            if (ddlImzaRota) {
                ddlImzaRota.bind("change", function (e) {
                    // Rota değiştiğinde son imzacıyı güncelle
                    EvrakOlustur.updateImzaciFromRota(this.value());
                });
            }

        },
        setData: function (data) {
            if (!data) return;

            // 1. Normal Inputlar
            $("#konu").val(data.konu || data.Konu);
            $("#EvrakId").val(data.id || data.Id); // Hidden ID alanı varsa

            // 2. Kendo ComboBox (Konu Kodu)
            var ddlKonuKodu = $("#KonuKoduId").data("kendoComboBox");
            if (ddlKonuKodu) ddlKonuKodu.value(data.konuKoduId || data.KonuKoduId);

            // 3. Kendo DropDownLists
            var ddlImzaRota = $("#ImzaRota").data("kendoDropDownList");
            if (ddlImzaRota) ddlImzaRota.value(data.imzaRotaId || data.ImzaRotaId);

            var ddlGizlilik = $("#gizlilik").data("kendoDropDownList");
            if (ddlGizlilik) ddlGizlilik.value(data.gizlilikDerecesi || data.GizlilikDerecesi);

            var ddlIvedilik = $("#ivedilik").data("kendoDropDownList");
            if (ddlIvedilik) ddlIvedilik.value(data.ivedilik || data.Ivedilik || data.ivedilikDurumu);

            // 4. Kendo Editors
            var editorGövde = $("#EvrakEditor").data("kendoEditor");
            if (editorGövde) editorGövde.value(data.icerik || data.Icerik || "");

            var editorAlt = $("#AltMetinEditor").data("kendoEditor");
            if (editorAlt) editorAlt.value(data.imzaAltindaOlanIcerik || data.ImzaAltindaOlanIcerik || "");
        },
        getData: function () {
         

            var ddlImzaRota = $("#ImzaRota").data("kendoDropDownList");
            var ddlKonuKodu = $("#KonuKoduId").data("kendoComboBox");
            var ddlGizlilik = $("#gizlilik").data("kendoDropDownList");
            var ddlIvedilik = $("#ivedilik").data("kendoDropDownList");
            var editorGövde = $("#EvrakEditor").data("kendoEditor");
            var editorAlt = $("#AltMetinEditor").data("kendoEditor");
                
            return {
                Id: parseInt($("#EvrakId").val()) || 0,
                Konu: $("#konu").val(),
                KonuKoduId: ddlKonuKodu ? parseInt(ddlKonuKodu.value()) : 0,
                Icerik: editorGövde.value(),
                ImzaAltindaOlanIcerik: editorAlt ? editorAlt.value() : "",
                ImzaRotaId: ddlImzaRota.value() ? parseInt(ddlImzaRota.value()) : 0,
                GizlilikDerecesi: ddlGizlilik ? parseInt(ddlGizlilik.value()) : 0,
                Ivedilik: ddlIvedilik ? parseInt(ddlIvedilik.value()) : 0
            };
        }
    };
})();   