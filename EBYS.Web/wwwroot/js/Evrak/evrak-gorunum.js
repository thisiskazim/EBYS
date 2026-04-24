var OnizlemeModule = (function () {
    var _imzaciAd = "";
    var _imzaciUnvan = "";

    return {
        setImzaci: function (ad, unvan) {
            _imzaciAd = ad;
            _imzaciUnvan = unvan;
        },

        verileriYukle: function () {
            try {
                // 1. Tüm modüllerden güncel verileri topla
                var bilgiler = EvrakBilgiModule.getData();
                var alicilar = AliciModule.getData() || [];
                var ilgiler = (typeof IlgilerModule !== "undefined") ? IlgilerModule.getData() : [];
                var ekler = (typeof EklerModule !== "undefined") ? EklerModule.getData() : [];
                console.log("veriler:", bilgiler, alicilar, ilgiler, ekler);
                // 2. Temel Alanları Bas
                $("#view-sayi").text("11428951-" + (bilgiler.KonuKoduId || "000"));
                $("#view-konu").text(bilgiler.Konu || "");

                // Kendo Editor İçerikleri
                var editorGövde = $("#EvrakEditor").data("kendoEditor");
                if (editorGövde) $("#view-icerik").html(editorGövde.value());

                var editorAlt = $("#AltMetinEditor").data("kendoEditor");
                if (editorAlt) $("#view-imza-alti-icerik").html(editorAlt.value());

                // 3. İlgiler (Resmi format: a, b, c...)
                if (ilgiler.length > 0) {
                    var ilgiHtml = "<strong>İlgi :</strong> ";
                    ilgiler.forEach((i, idx) => {
                        var harf = String.fromCharCode(97 + idx) + ")";
                        ilgiHtml += (idx === 0 ? "" : "<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + harf + " " + i.IlgiMetni;
                    });
                    $("#view-ilgiler-alani").html(ilgiHtml).show();
                } else { $("#view-ilgiler-alani").hide(); }

                // 4. Muhatap ve Dağıtım Mantığı
                if (alicilar.length > 1) {
                    $("#view-muhatap-baslik").html("<strong>DAĞITIM YERLERİNE</strong>");
                    $("#view-dagitim-alani").show();

                    var geregiHtml = "<strong>Gereği:</strong><br>", bilgiHtml = "<strong>Bilgi:</strong><br>";
                    var gVar = false, bVar = false;

                    alicilar.forEach(a => {
                        if (a.IsBilgi) { bilgiHtml += "- " + a.MuhatapAdi + "<br>"; bVar = true; }
                        else { geregiHtml += "- " + a.MuhatapAdi + "<br>"; gVar = true; }
                    });
                    $("#view-geregi-listesi").html(gVar ? geregiHtml : "").toggle(gVar);
                    $("#view-bilgi-listesi").html(bVar ? bilgiHtml : "").toggle(bVar);
                } else if (alicilar.length === 1) {
                    $("#view-muhatap-baslik").html("<strong>" + alicilar[0].MuhatapAdi.toUpperCase() + "</strong>");
                    $("#view-dagitim-alani").hide();
                }

                // 5. Dinamik İmza (Rotadan gelen)
                $("#view-imza-ad").text((_imzaciAd || "Süleyman ARSLAN").toUpperCase());
                $("#view-imza-unvan").text(_imzaciUnvan || "Birlik Başkanı");

                // 6. Ekler
                if (ekler.length > 0) {
                    var ekHtml = "<strong>Ek :</strong><br>";
                    ekler.forEach((e, idx) => {
                        ekHtml += (idx + 1) + "- " + e.Ad + "<br>";
                    });
                    $("#view-ekler-listesi").html(ekHtml).show();
                } else { $("#view-ekler-listesi").hide(); }

            } catch (err) {
                console.error("Önizleme yüklenirken hata oluştu:", err);
            }
        },

        pdfIndir: function () {
            kendo.drawing.drawDOM($(".a4-sayfa"), {
                paperSize: "A4",
                margin: { top: "1cm", bottom: "1cm" },
                scale: 0.8,
                avoidLinks: true
            })
                .then(function (group) {
                    return kendo.drawing.exportPDF(group);
                })
                .done(function (data) {
                    kendo.saveAs({
                        dataURI: data,
                        fileName: "Evrak_Onizleme.pdf"
                    });
                });
        }
    };
})();
