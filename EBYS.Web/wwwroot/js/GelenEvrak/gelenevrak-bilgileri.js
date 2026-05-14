var GelenEvrakBilgiModule = (function () {
    return {
        init: function () {
            // Gelen evrakta gerekirse özel tetikleyiciler buraya eklenebilir
        },

        setData: function (data) {
            if (!data) return;

            // 1. Normal Inputlar ve Checkbox
            $("#EvrakId").val(data.id || data.Id || 0);
            $("#Konu").val(data.konu || data.Konu || "");
            $("#EvrakSayisi").val(data.evrakSayisi || data.EvrakSayisi || "");
            $("#DilekceMi").prop('checked', data.dilekceMi || data.DilekceMi || false);

            // 2. Kendo ComboBoxes (Muhatap ve Konu Kodu)
            var cbMuhatap = $("#MuhatapId").data("kendoComboBox");
            if (cbMuhatap) cbMuhatap.value(data.muhatapId || data.MuhatapId || "");

            var cbKonuKodu = $("#KonuKodu").data("kendoComboBox");
            if (cbKonuKodu) cbKonuKodu.value(data.konuKodu || data.KonuKodu || "");

            // 3. Kendo DatePickers
            var dpEvrakTarihi = $("#EvrakTarihi").data("kendoDatePicker");
            if (dpEvrakTarihi) dpEvrakTarihi.value(data.evrakTarihi || data.EvrakTarihi || new Date());

            var dpDefterTarihi = $("#DefterTarihi").data("kendoDatePicker");
            if (dpDefterTarihi) dpDefterTarihi.value(data.defterTarihi || data.DefterTarihi || new Date());

            var dpCevapIstenen = $("#CevapIstenenTarih").data("kendoDatePicker");
            if (dpCevapIstenen) dpCevapIstenen.value(data.cevapIstenenTarih || data.CevapIstenenTarih || null);

            // 4. Kendo DropDownLists (Gizlilik ve İvedilik)
            var ddlGizlilik = $("#GizlilikDerecesi").data("kendoDropDownList");
            if (ddlGizlilik) ddlGizlilik.value(data.gizlilikDerecesi ?? data.GizlilikDerecesi ?? 0);

            var ddlIvedilik = $("#IvedilikDerecesi").data("kendoDropDownList");
            if (ddlIvedilik) ddlIvedilik.value(data.ivedilikDerecesi ?? data.IvedilikDerecesi ?? 0);
        },

        getData: function () {
            // Kendo nesnelerini al
            var cbMuhatap = $("#MuhatapId").data("kendoComboBox");
            var cbKonuKodu = $("#KonuKodu").data("kendoComboBox");
            var dpEvrakTarihi = $("#EvrakTarihi").data("kendoDatePicker");
            var dpDefterTarihi = $("#DefterTarihi").data("kendoDatePicker");
            var dpCevapIstenen = $("#CevapIstenenTarih").data("kendoDatePicker");
            var ddlGizlilik = $("#GizlilikDerecesi").data("kendoDropDownList");
            var ddlIvedilik = $("#IvedilikDerecesi").data("kendoDropDownList");

            return {
                Id: parseInt($("#EvrakId").val()) || 0,
                Konu: $("#Konu").val(),
                EvrakSayisi: $("#EvrakSayisi").val(),
                EvrakTarihi: kendo.toString(dpEvrakTarihi.value(), "yyyy-MM-dd"),
                DefterTarihi: kendo.toString(dpDefterTarihi.value(), "yyyy-MM-dd"),
                CevapIstenenTarih: dpCevapIstenen.value() ? kendo.toString(dpCevapIstenen.value(), "yyyy-MM-dd") : null,
                DilekceMi: $("#DilekceMi").is(":checked"),
                MuhatapId: cbMuhatap ? parseInt(cbMuhatap.value()) : 0,
                KonuKodu: cbKonuKodu ? cbKonuKodu.value() : "",
                GizlilikDerecesi: ddlGizlilik ? parseInt(ddlGizlilik.value()) : 0,
                IvedilikDerecesi: ddlIvedilik ? parseInt(ddlIvedilik.value()) : 0
            };
        }
    };
})();