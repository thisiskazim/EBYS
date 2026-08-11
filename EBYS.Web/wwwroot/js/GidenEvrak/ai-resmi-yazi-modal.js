var AiResmiYaziModal = (function () {
    var modalInstance = null;
    var generatedKonu = "";
    var generatedIcerik = "";

    function getYaziTuruValue() {
        var ddl = $("#AiYaziTuru").data("kendoDropDownList");
        return ddl ? ddl.value() : "";
    }

    function getYaziUzunluguValue() {
        var ddl = $("#AiYaziUzunlugu").data("kendoDropDownList");
        return ddl ? ddl.value() : "2";
    }

    function plainTextToHtml(text) {
        if (!text) return "";

        var escaped = text
            .replace(/&/g, "&amp;")
            .replace(/</g, "&lt;")
            .replace(/>/g, "&gt;");

        return escaped
            .split(/\n{2,}/)
            .map(function (paragraph) {
                return "<p>" + paragraph.replace(/\n/g, "<br/>") + "</p>";
            })
            .join("");
    }

    function hasGeneratedContent() {
        return !!(generatedKonu || generatedIcerik);
    }

    function setLoading(isLoading) {
        var $overlay = $("#aiModalLoadingOverlay");
        var $donustur = $("#btnAiDonustur");
        var $aktar = $("#btnAiEvrakaAktar");
        var $vazgec = $("#aiResmiYaziModal").find('[data-bs-dismiss="modal"]');

        if (isLoading) {
            $overlay.removeClass("d-none");
            $donustur.prop("disabled", true);
            $aktar.prop("disabled", true);
            $vazgec.prop("disabled", true);
        } else {
            $overlay.addClass("d-none");
            $donustur.prop("disabled", false);
            $vazgec.prop("disabled", false);
            $aktar.prop("disabled", !hasGeneratedContent());
        }
    }

    function resetModal() {
        generatedKonu = "";
        generatedIcerik = "";
        $("#aiKullaniciNotu").val("");
        $("#aiOnizlemeKonu").val("");
        $("#aiOnizlemeIcerik").val("");
        $("#aiKarakterSayaci").text("0");
        $("#btnAiEvrakaAktar").prop("disabled", true);
        setLoading(false);

        var ddl = $("#AiYaziTuru").data("kendoDropDownList");
        if (ddl) ddl.value("");

        var ddlUzunluk = $("#AiYaziUzunlugu").data("kendoDropDownList");
        if (ddlUzunluk) ddlUzunluk.value("2");
    }

    function openModal() {
        resetModal();
        if (modalInstance) {
            modalInstance.show();
        }
    }

    function validateForm() {
        var yaziTuru = getYaziTuruValue();
        var not = ($("#aiKullaniciNotu").val() || "").trim();

        if (!yaziTuru) {
            showNotification("Lütfen yazı türü seçiniz.", "warning");
            return false;
        }

        if (not.length < 10) {
            showNotification("Kullanıcı notu en az 10 karakter olmalıdır.", "warning");
            return false;
        }

        return true;
    }

    function donustur() {
        if (!validateForm()) return;

        var request = {
            YaziTuru: parseInt(getYaziTuruValue(), 10),
            YaziUzunlugu: parseInt(getYaziUzunluguValue(), 10),
            TaslakMetin: $("#aiKullaniciNotu").val().trim()
        };

        setLoading(true);
        generatedKonu = "";
        generatedIcerik = "";

        ApiService.postJson("ResmiYazi/Generate", request)
            .done(function (response) {
                generatedKonu = response.Konu || response.konu || "";
                generatedIcerik = response.ResmiMetin || response.resmiMetin || "";

                $("#aiOnizlemeKonu").val(generatedKonu);
                $("#aiOnizlemeIcerik").val(generatedIcerik);
                $("#btnAiEvrakaAktar").prop("disabled", !hasGeneratedContent());

                if (hasGeneratedContent()) {
                    showNotification("Konu ve içerik başarıyla oluşturuldu.", "success");
                } else {
                    showNotification("AI geçerli bir konu veya içerik üretemedi.", "warning");
                }
            })
            .fail(function () {
                generatedKonu = "";
                generatedIcerik = "";
                $("#aiOnizlemeKonu").val("");
                $("#aiOnizlemeIcerik").val("");
                $("#btnAiEvrakaAktar").prop("disabled", true);
            })
            .always(function () {
                setLoading(false);
            });
    }

    function evrakaAktar() {
        if (!hasGeneratedContent()) {
            showNotification("Aktarılacak konu veya içerik bulunamadı. Önce dönüştürme işlemini yapınız.", "warning");
            return;
        }

        if (generatedKonu) {
            $("#konu").val(generatedKonu);
        }

        if (generatedIcerik) {
            var editor = $("#EvrakEditor").data("kendoEditor");
            if (!editor) {
                showNotification("Evrak editörü bulunamadı.", "error");
                return;
            }
            editor.value(plainTextToHtml(generatedIcerik));
        }

        if (modalInstance) {
            modalInstance.hide();
        }

        showNotification("Konu ve içerik evraka aktarıldı.", "success");

        var bilgilerTab = document.querySelector('#bilgiler-tab');
        if (bilgilerTab) {
            bootstrap.Tab.getOrCreateInstance(bilgilerTab).show();
        }
    }

    function bindEvents() {
        $("#btnAiTaslakOlustur").on("click", openModal);
        $("#btnAiDonustur").on("click", donustur);
        $("#btnAiEvrakaAktar").on("click", evrakaAktar);

        $("#aiKullaniciNotu").on("input", function () {
            $("#aiKarakterSayaci").text(this.value.length);
        });

        var modalEl = document.getElementById("aiResmiYaziModal");
        if (modalEl) {
            modalEl.addEventListener("hidden.bs.modal", resetModal);
        }
    }

    return {
        init: function () {
            var modalEl = document.getElementById("aiResmiYaziModal");
            if (!modalEl) return;

            modalInstance = bootstrap.Modal.getOrCreateInstance(modalEl);
            bindEvents();
        }
    };
})();
