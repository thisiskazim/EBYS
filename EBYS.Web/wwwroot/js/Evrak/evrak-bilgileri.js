var EvrakBilgiModule = (function () {
    return {
        init: function () {

        },

        getData: function () {
         
            var ddlImzaRota = $("#ImzaRota").data("kendoDropDownList");
            var ddlKonuKodu = $("#KonuKoduId").data("kendoComboBox");
            var ddlGizlilik = $("#gizlilik").data("kendoDropDownList");
            var ddlIvedilik = $("#ivedilik").data("kendoDropDownList");
            var editorGövde = $("#EvrakEditor").data("kendoEditor");
            var editorAlt = $("#AltMetinEditor").data("kendoEditor");
                
            return {
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