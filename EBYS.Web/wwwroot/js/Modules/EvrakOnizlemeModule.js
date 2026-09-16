var EvrakOnizlemeModule = (function () {
    var _dialog = null;
    var _apiBaseUrl = "https://localhost:7060/api/";
    var _currentObjectUrl = null;

    var _getDialogSize = function () {
        return {
            width: Math.min(1300, Math.max(320, window.innerWidth - 32)),
            height: Math.min(800, Math.max(420, window.innerHeight - 32))
        };
    };

    var _releasePreview = function () {
        $("#pdf-frame-popup").attr("src", "about:blank");
        $("#onizleme-yukleniyor").hide();

        if (_currentObjectUrl) {
            URL.revokeObjectURL(_currentObjectUrl);
            _currentObjectUrl = null;
        }
    };

    var _init = function () {
        if ($("#onizlemeDialog").data("kendoDialog")) {
            _dialog = $("#onizlemeDialog").data("kendoDialog");
            return;
        }

        var size = _getDialogSize();
        _dialog = $("#onizlemeDialog").kendoDialog({
            width: size.width,
            height: size.height,
            title: "Evrak Detay Önizleme",
            closable: true,
            modal: true,
            visible: false,
            actions: [{ text: "Kapat" }],
            close: _releasePreview
        }).data("kendoDialog");

        _dialog.wrapper.addClass("ebys-preview-dialog");

        $(window).off("resize.ebysPreview").on("resize.ebysPreview", function () {
            if (!_dialog || !_dialog.wrapper.is(":visible")) return;

            var resized = _getDialogSize();
            _dialog.setOptions({ width: resized.width, height: resized.height });
        });
    };

    return {
        ac: function (ekId, evrakTipi) {
            _init();

            var size = _getDialogSize();
            _dialog.setOptions({ width: size.width, height: size.height });
            _dialog.open();
            _releasePreview();
            $("#onizleme-yukleniyor").show();

            var url = _apiBaseUrl + "EvrakOnizleme/EkGoruntule/" + evrakTipi + "/" + ekId;
            var xhr = new XMLHttpRequest();
            xhr.open("GET", url, true);
            xhr.setRequestHeader("Authorization", "Bearer " + localStorage.getItem("token"));
            xhr.responseType = "blob";

            xhr.onload = function () {
                if (this.status !== 200) return;

                if (_currentObjectUrl) {
                    URL.revokeObjectURL(_currentObjectUrl);
                }

                var blob = new Blob([this.response], { type: "application/pdf" });
                _currentObjectUrl = URL.createObjectURL(blob);

                setTimeout(function () {
                    $("#pdf-frame-popup").attr("src", _currentObjectUrl + "#view=FitH");
                    $("#onizleme-yukleniyor").hide();
                }, 100);
            };

            xhr.send();
        },

        getIconByExtension: function (ext) {
            if (!ext) return "fas fa-file text-secondary";
            ext = ext.toLowerCase();
            if (ext.includes("pdf")) return "fas fa-file-pdf text-danger";
            if (ext.includes("xls")) return "fas fa-file-excel text-success";
            if (ext.includes("doc")) return "fas fa-file-word text-primary";
            if (ext.includes("jpg") || ext.includes("png")) return "fas fa-file-image text-warning";
            return "fas fa-file text-secondary";
        },

        toggleEkler: function (element) {
            var list = $(element).find(".ek-listesi-gizli");
            var icon = $(element).find(".fa-chevron-down, .fa-chevron-up");

            list.slideToggle("fast");
            icon.toggleClass("fa-chevron-down fa-chevron-up");
        }
    };
})();
